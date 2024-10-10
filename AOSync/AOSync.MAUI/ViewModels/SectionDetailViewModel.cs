using System.Windows.Input;
using AOSync.BL.Services;
using AOSync.DAL.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AOSync.MAUI.ViewModels;

[QueryProperty(nameof(SectionId), nameof(SectionId))]
public partial class SectionDetailViewModel : BaseViewModel
{
    [ObservableProperty] private SectionEntity _section;

    private IServiceProvider _serviceProvider;

    [ObservableProperty] private IEnumerable<TaskEntity> _tasks;

    [ObservableProperty] private string sectionId;

    public SectionDetailViewModel()
    {
        TaskSelectedCommand = new Command<TaskEntity>(OnTaskSelected);
    }

    public ICommand TaskSelectedCommand { get; }

    public async void Initialize(IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        _serviceProvider = serviceProvider;
        dataReloadService.RegisterReloadAction(LoadTasks);
    }

    public async Task OnAppearing()
    {
        await LoadTasks();
        await LoadSectionDetail();
    }

    private async Task LoadSectionDetail()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = _serviceProvider.CreateScope();
        var sectionService = scope.ServiceProvider.GetService<ISectionService>();
        Section = await sectionService.GetByIdAsync(new Guid(sectionId));
    }

    private async Task LoadTasks()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("ServiceProvider not initialized.");

        using var scope = _serviceProvider.CreateScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
        Tasks = await taskService.GetTasksBySectionId(new Guid(sectionId));
    }

    private async void OnTaskSelected(TaskEntity selectedTask)
    {
        if (selectedTask != null)
            await Shell.Current.GoToAsync(
                $"///{Routes.ProjectListView}/{Routes.ProjectDetailView}/{Routes.SectionDetailView}/{Routes.TaskDetailView}?TaskId={selectedTask.Id}");
    }
}