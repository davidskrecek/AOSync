using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public partial class ProjectDetailView : ContentPage
{
    private readonly ProjectDetailViewModel _viewModel;

    public ProjectDetailView()
    {
        InitializeComponent();
        _viewModel = new ProjectDetailViewModel();
        _viewModel.Initialize(App._serviceProvider);
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