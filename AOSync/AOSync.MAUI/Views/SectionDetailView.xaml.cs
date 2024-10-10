using AOSync.BL.Services;
using AOSync.MAUI.ViewModels;

namespace AOSync.MAUI.Views;

public partial class SectionDetailView : ContentPage
{
    private readonly SectionDetailViewModel _viewModel;

    public SectionDetailView()
    {
        InitializeComponent();
        _viewModel = new SectionDetailViewModel();
        _viewModel.Initialize(App._serviceProvider, App._serviceProvider.GetRequiredService<DataReloadService>());
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