using System.Collections.ObjectModel;
using AOSync.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using AOSync.Services;

namespace MAUI.ViewModels
{
    public partial class UserListViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IEnumerable<UserEntity> _users = [];

        public async void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
        {
            _serviceProvider = serviceProvider;
            dataReloadService.RegisterReloadAction(LoadUsers);
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("ServiceProvider not initialized.");

            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            Users = await userService.GetAllAsync();
        }
    }
}
