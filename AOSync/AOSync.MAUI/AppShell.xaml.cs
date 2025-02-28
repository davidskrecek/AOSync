using AOSync.MAUI.Views;

namespace AOSync.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute($"///ProjectListView", typeof(ProjectListView));
        Routing.RegisterRoute($"///ProjectListView/ProjectDetailView", typeof(ProjectDetailView));
    }
}