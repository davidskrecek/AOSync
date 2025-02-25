using AOSync.MAUI.Views;

namespace AOSync.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute($"///ProjectListView", typeof(ProjectListView));
        Routing.RegisterRoute($"///ProjectListView/ProjectDetailView", typeof(ProjectDetailView));
        Routing.RegisterRoute($"///ProjectListView/ProjectDetailView/SectionDetailView",
            typeof(SectionDetailView));
        Routing.RegisterRoute(
            $"///ProjectListView/ProjectDetailView/SectionDetailView/TaskDetailView",
            typeof(TaskDetailView));
        Routing.RegisterRoute($"///UserListView", typeof(UserListView));
        Routing.RegisterRoute($"///TimeSheetListView", typeof(TimeSheetListView));
    }
}