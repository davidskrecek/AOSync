using System.Windows.Input;
using AOSync.BL.Services;
using AOSync.DAL.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AOSync.MAUI.ViewModels;

[QueryProperty(nameof(ProjectId), nameof(ProjectId))]
public partial class ProjectDetailViewModel : BaseViewModel
{
    [ObservableProperty] private ProjectEntity _project;

    [ObservableProperty] private IEnumerable<SectionEntity> _sections;

    private IServiceProvider _serviceProvider;

    public ProjectDetailViewModel()
    {
        SubmitChangesCommand = new Command(OnSubmitChanges);
        SectionSelectedCommand = new Command<SectionEntity>(OnSectionSelected);
    }

    public string ProjectId { get; set; }
    public ICommand SectionSelectedCommand { get; }
    public ICommand SubmitChangesCommand { get; }

    public void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        _serviceProvider = serviceProvider;
        dataReloadService.RegisterReloadAction(LoadProjectDetail);
        dataReloadService.RegisterReloadAction(LoadSections);
    }

    public async void OnAppearing()
    {
        await LoadProjectDetail();
        await LoadSections();
    }

    private async Task LoadProjectDetail()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = _serviceProvider.CreateScope();
        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
        Project = await projectService.GetByIdAsync(new Guid(ProjectId));
    }

    private async Task LoadSections()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = _serviceProvider.CreateScope();
        var sectionService = scope.ServiceProvider.GetRequiredService<ISectionService>();
        Sections = await sectionService.GetSectionsByProjectId(new Guid(ProjectId));
    }

    private async void OnSectionSelected(SectionEntity selectedSection)
    {
        if (selectedSection != null)
            await Shell.Current.GoToAsync(
                $"///{Routes.ProjectListView}/{Routes.ProjectDetailView}/{Routes.SectionDetailView}?SectionId={selectedSection.Id}");
    }

    private async void OnSubmitChanges()
    {
        ProjectEntity newProjectEntity = new()
        {
            Id = Project.Id,
            ExternalId = Project.ExternalId,
            Name = Project.Name,
            Archived = Project.Archived,
            IsChanged = true
        };
        using var scope = _serviceProvider.CreateScope();
        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
        await projectService.AddOrUpdateAsync(newProjectEntity);
    }
}