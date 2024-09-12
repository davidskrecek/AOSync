using AOSync.Services;
using MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace MAUI.Views
{
    public partial class SectionDetailView : ContentPage
    {
        private readonly SectionDetailViewModel _viewModel;

        public SectionDetailView()
        {
            InitializeComponent();
            _viewModel = new SectionDetailViewModel();
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
}