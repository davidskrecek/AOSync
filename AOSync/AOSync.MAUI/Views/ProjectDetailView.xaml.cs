using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public class ProjectDetailView : ContentPage
{
    private readonly ProjectDetailViewModel _viewModel;

    public ProjectDetailView()
    {
        InitializeComponent();
        _viewModel = new ProjectDetailViewModel();
        _viewModel.Initialize(App.ServiceProvider, App.ServiceProvider.GetRequiredService<DataReloadService>());
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");
        return true;
    }
}