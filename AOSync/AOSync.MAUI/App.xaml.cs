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
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
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
