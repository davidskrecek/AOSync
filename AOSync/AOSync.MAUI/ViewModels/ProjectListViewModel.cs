using System.Windows.Input;
using AOSync.BL.Services;
using AOSync.DAL.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace AOSync.MAUI.ViewModels;

public class ProjectListViewModel : BaseViewModel
{
    private IProjectService _iProjectService  { get; set; }
    public IEnumerable<ProjectEntity> Projects { get; private set; }

    public ProjectEntity SelectedProject { get; private set; }


    public ProjectListViewModel()
    {
        ProjectSelectedCommand = new Command<ProjectEntity>(OnProjectSelected);
    }

    public ICommand ProjectSelectedCommand { get; }

    public void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        ServiceProvider = serviceProvider;
        _iProjectService = ServiceProvider.GetRequiredService<IProjectService>();
        dataReloadService.RegisterReloadAction(LoadProjects);
    }

    public async void OnAppearing()
    {
        await LoadProjects();
    }

    private async Task LoadProjects()
    {
        if (_iProjectService == null)
            throw new InvalidOperationException("_iProjectService not initialized.");

        Projects = await _iProjectService.GetAllAsync();
    }

    private async void OnProjectSelected(ProjectEntity selectedProject)
    {
            await Shell.Current.GoToAsync(
                $"///ProjectListView/ProjectDetailView?ProjectId={selectedProject.Id}");
    }
}