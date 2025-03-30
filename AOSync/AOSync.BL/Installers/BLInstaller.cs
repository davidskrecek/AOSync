using AOSync.BL.ProcessingModules;
using AOSync.BL.Services;
using AOSync.BL.Services.Synchronization;
using AOSync.BL.Services.Synchronization.Interfaces;
using AOSync.BL.Facades;
using AOSync.BL.Facades.Interfaces;
using AOSync.COMMON.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using AOSync.APICLIENT;

namespace AOSync.BL.Installers
{
    public class BLInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(config => config.AddConsole());
            serviceCollection.AddScoped<ISynchronizationService, SynchronizationService>();
            serviceCollection.AddHostedService<SyncBackgroundService>();

            serviceCollection.AddScoped<SetChangesProcessor>();
            serviceCollection.AddScoped<SyncGetChangesResultProcessor>();
            serviceCollection.AddScoped<SyncGetInitialChangesResultProcessor>();

            serviceCollection.AddScoped<ISynchronizationApiClient, SynchronizationApiClient>();

            RegisterServices(serviceCollection);
            RegisterFacades(serviceCollection);
        }

        private void RegisterServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType is { IsGenericType: true } &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(ServiceBase<>))
                .ToList();

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => 
                    i != typeof(IServiceBase<>) && i.GetInterfaces().Any(baseInterface => baseInterface.IsGenericType && 
                        baseInterface.GetGenericTypeDefinition() == typeof(IServiceBase<>)));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }
        }

        private void RegisterFacades(IServiceCollection services)
        {
            services.AddScoped<IProjectFacade, ProjectFacade>();
        }
    }
}