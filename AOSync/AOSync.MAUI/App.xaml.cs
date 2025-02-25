using AOSync.BL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace AOSync.MAUI;

public partial class App : Application
{
    private readonly ILogger<App> _logger;
    public static IServiceProvider _serviceProvider { get; private set; } = null!;
    private Timer _timer { get; set; } = null!;

    public App(IServiceProvider serviceProvider, ILogger<App> logger, IConfiguration configuration)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _logger = logger;

        StartBackgroundTask();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }


    // Start the background task
    private async void StartBackgroundTask()
    {
        using var scope = _serviceProvider.CreateScope();
        var businessManager = scope.ServiceProvider.GetRequiredService<BusinessManager>();
        await businessManager.GetInitialChanges();

        // Initialize the background task using a Timer
        _timer = new Timer(async state =>
        {
            try
            {
                await businessManager.GetChanges();
                await businessManager.SetChanges();
                _logger.LogInformation("Background task is running.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the background task.");
            }
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    // MAUI uses OnStart and OnStop instead of OnSleep and OnResume
    protected override void OnStart()
    {
        base.OnStart();
        // Restart the timer when the app starts
        _timer?.Change(TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    protected void OnStop()
    {
        // Stop the timer when the app stops
        _timer?.Change(Timeout.Infinite, 0);
    }
}
