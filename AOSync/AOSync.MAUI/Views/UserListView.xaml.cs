using AOSync.BL.Services;
using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public partial class UserListView : ContentPage
{
    private readonly UserListViewModel _viewModel;

    public UserListView()
    {
        InitializeComponent();
        _viewModel = App._serviceProvider.GetRequiredService<UserListViewModel>();
        _viewModel.Initialize(App._serviceProvider, App._serviceProvider.GetRequiredService<DataReloadService>());
        BindingContext = _viewModel;
    }
}