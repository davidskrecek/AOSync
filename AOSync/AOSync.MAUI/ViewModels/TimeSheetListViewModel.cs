using CommunityToolkit.Mvvm.ComponentModel;

namespace AOSync.MAUI.ViewModels;

public partial class TimeSheetListViewModel : BaseViewModel
{
    [ObservableProperty] private IEnumerable<TimeSheetEntity> _timeSheets = [];

    public async void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        _serviceProvider = serviceProvider;
        dataReloadService.RegisterReloadAction(LoadTimeSheets);
        await LoadTimeSheets();
    }

    private async Task LoadTimeSheets()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = _serviceProvider.CreateScope();
        var timeSheetService = scope.ServiceProvider.GetRequiredService<ITimeSheetService>();
        TimeSheets = await timeSheetService.GetAllAsync();
    }
}