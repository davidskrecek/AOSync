using AOSync.BL;
using AOSync.BL.Services;
using AOSync.COMMON;
using AOSync.COMMON.ApiClient;
using AOSync.COMMON.Converters;
using AOSync.DAL.DB;
using AOSync.MAUI.Extensions;
using AOSync.MAUI.ViewModels;
using AOSync.MAUI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AOSync.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        ConfigureServices(builder);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        return builder.Build();
    }

    private static void ConfigureServices(MauiAppBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        var profile = configuration["ConfigurationProfile"] ?? "Alfa";
        var profileConfig = configuration.GetSection(profile);

        var services = builder.Services;

        // Add the configuration object
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();
        
        // Add HttpClient with apikey and base url configured
        services.AddHttpClient<HttpClient>(client =>
        {
            client.BaseAddress = new Uri(profileConfig["BaseUrl"]!);
            client.DefaultRequestHeaders.Add("apikey", profileConfig["APIKey"]);
        });

        // Add ApplicationDbContext and configure MySQL connection
        services.AddDbContext<AOSyncDbContext>(options =>
            options.UseMySql(profileConfig.GetConnectionString("ConnectionString"),
                new MySqlServerVersion(new Version(8, 0, 21))
            )
        );

        // Add Business Logic layer services
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISectionService, SectionService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITimesheetService, TimesheetService>();
        services.AddScoped<IChangeTrackingService, ChangeTrackingService>();

        // Add BusinessManager and ApiCommunicator
        services.AddScoped<BusinessManager>();
        services.AddScoped<ISynchronizationApiClient, SynchronizationApiClient>();
        

        // Add the DataReloadService
        services.AddSingleton<DataReloadService>();

        // Register Views and ViewModels dynamically
        Registrar.RegisterRoutes();
        Registrar.RegisterViewModels(services);

        // Add JSON serialization options (optional)
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ChangesToDefClassConverter());
                options.SerializerSettings.Converters.Add(new DateTimeConverter());
            });
        
        
    }
}