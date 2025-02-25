using AOSync.BL.Services;
using AOSync.DAL.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AOSync.MAUI.ViewModels;

public partial class TimeSheetListViewModel : BaseViewModel
{
    public ICollection<TimesheetEntity> TimeSheets  { get; private set; } = [];

    public async void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        ServiceProvider = serviceProvider;
        dataReloadService.RegisterReloadAction(LoadTimeSheets);
        await LoadTimeSheets();
    }

    private async Task LoadTimeSheets()
    {
        if (ServiceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = ServiceProvider.CreateScope();
        var timeSheetService = scope.ServiceProvider.GetRequiredService<ITimesheetService>();
        TimeSheets = await timeSheetService.GetAllAsync();
    }
}