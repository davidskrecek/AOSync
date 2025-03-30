using AOSync.BL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.Management.Deployment;

namespace AOSync.MAUI;

public partial class App : Application
{
    private readonly ILogger<App> _logger;
    public static IServiceProvider _serviceProvider { get; private set; } = null!;
    private readonly SyncBackgroundService _syncBackgroundService;

    public App(IServiceProvider serviceProvider, ILogger<App> logger, IConfiguration configuration)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _logger = logger;

        _syncBackgroundService = _serviceProvider.GetRequiredService<SyncBackgroundService>();
        _syncBackgroundService.StartAsync(CancellationToken.None);

        // Log an info message to indicate app startup
        _logger.LogInformation("App initialized.");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    // No need for the timer here, the background service will handle periodic execution
    protected override void OnStart()
    {
        base.OnStart();
        _logger.LogInformation("App started, SyncBackgroundService should run in the background.");
    }

    // Optional, if you want to handle app stop or shutdown manually
    protected void OnStop()
    {
        _logger.LogInformation("App stopping.");
    }
}