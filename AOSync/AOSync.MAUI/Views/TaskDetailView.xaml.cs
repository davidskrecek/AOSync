using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public class TaskDetailView : ContentPage
{
    private readonly TaskDetailViewModel _viewModel;

    public TaskDetailView()
    {
        InitializeComponent();
        _viewModel = new TaskDetailViewModel();
        _viewModel.Initialize(App.ServiceProvider, App.ServiceProvider.GetRequiredService<DataReloadService>());
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");
        return true;
    }
}