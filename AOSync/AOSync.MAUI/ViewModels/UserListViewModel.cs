using AOSync.BL.Services;
using AOSync.DAL.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AOSync.MAUI.ViewModels;

public partial class UserListViewModel : BaseViewModel
{
    public IEnumerable<UserEntity> Users { get; private set; } = [];

    public async void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        ServiceProvider = serviceProvider;
        dataReloadService.RegisterReloadAction(LoadUsers);
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        if (ServiceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = ServiceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        Users = await userService.GetAllAsync();
    }
}