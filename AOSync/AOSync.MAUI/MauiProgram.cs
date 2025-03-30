using AOSync.BL;
using AOSync.COMMON;
using AOSync.COMMON.Converters;
using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Repositories;
using AOSync.DAL.Repositories.Interfaces;
using AOSync.MAUI.Extensions;
using AOSync.MAUI.ViewModels;
using AOSync.MAUI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using AOSync.BL.Installers;
using AOSync.COMMON.Installers;
using AOSync.DAL.Installers;

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

        // Determine which profile to use; default to "Alfa" if not set.
        var profile = configuration["ConfigurationProfile"] ?? "Alfa";
        var profileSection = configuration.GetSection(profile);

        var services = builder.Services;

        // Register the global configuration
        services.AddSingleton<IConfiguration>(configuration);

        // Manually configure and register HttpClient as a singleton
        services.AddSingleton<HttpClient>(provider =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(profileSection["BaseUrl"]!),
            };
            client.DefaultRequestHeaders.Add("apikey", profileSection["APIKey"]);
            return client;
        });

        // Add ApplicationDbContext with MySQL connection
        services.AddDbContextFactory<AOSyncDbContext>(options =>
            options.UseMySql(profileSection["ConnectionString"],
                new MySqlServerVersion(new Version(8, 0, 21))
            ), ServiceLifetime.Scoped
        );

        // Register the profile section directly as a configuration section
        services.AddSingleton<IConfigurationSection>(profileSection);

        // Dynamically discover and call all installers (DAL, BL)
        CallAllInstallers(services);

        // Register SyncBackgroundService as a hosted service
        services.AddSingleton<SyncBackgroundService>(); // Optional: If you need direct access
        services.AddHostedService<SyncBackgroundService>();  // Register as a hosted service

        // Add logging
        services.AddLogging();

        // Register Views and ViewModels dynamically
        Registrar.RegisterRoutes();
        Registrar.RegisterViewModels(services);

        // Add JSON serialization options
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ChangesToDefClassConverter());
                options.SerializerSettings.Converters.Add(new DateTimeConverter());
            });
    }

    private static void CallAllInstallers(IServiceCollection services)
    {
        List<IInstaller> installers = new()
        {
            new DALInstaller(),
            new BLInstaller()
        };

        foreach (var installer in installers)
        {
            installer.Install(services);
        }
    }
}