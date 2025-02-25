using AOSync.BL.Services;
using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public partial class TimeSheetListView : ContentPage
{
    private readonly TimeSheetListViewModel _viewModel;

    public TimeSheetListView()
    {
        InitializeComponent();
        _viewModel = App._serviceProvider.GetRequiredService<TimeSheetListViewModel>();
        _viewModel.Initialize(App._serviceProvider, App._serviceProvider.GetRequiredService<DataReloadService>());
        BindingContext = _viewModel;
    }
}