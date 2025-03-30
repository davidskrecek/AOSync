using System.Windows.Input;
using AOSync.BL.Facades.Interfaces;
using AOSync.BL.Services.Synchronization.Interfaces;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace AOSync.MAUI.ViewModels;

public class ProjectListViewModel : BaseViewModel
{
    private IProjectFacade _projectFacade  { get; set; }
    public IEnumerable<ProjectEntity> Projects { get; private set; }

    public ProjectEntity SelectedProject { get; private set; }


    public ProjectListViewModel()
    {
        ProjectSelectedCommand = new Command<ProjectEntity>(OnProjectSelected);
    }

    public ICommand ProjectSelectedCommand { get; }

    public void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        _projectFacade = ServiceProvider.GetRequiredService<IProjectFacade>();
    }

    public async void OnAppearing()
    {
        await LoadProjects();
    }

    private async Task LoadProjects()
    {
        Projects = await _projectFacade.GetAllAsync();
    }

    private async void OnProjectSelected(ProjectEntity selectedProject)
    {
            await Shell.Current.GoToAsync(
                $"///ProjectListView/ProjectDetailView?ProjectId={selectedProject.Id}");
    }
}