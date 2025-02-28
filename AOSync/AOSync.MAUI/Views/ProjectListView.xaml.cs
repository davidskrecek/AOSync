using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public partial class ProjectListView : ContentPage
{
    private readonly ProjectListViewModel _viewModel;

    public ProjectListView()
    {
        InitializeComponent();
        _viewModel = App._serviceProvider.GetRequiredService<ProjectListViewModel>();
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