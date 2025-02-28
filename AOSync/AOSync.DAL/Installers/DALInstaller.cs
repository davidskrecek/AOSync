using AOSync.COMMON.Installers;
using AOSync.DAL.Repositories;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace AOSync.DAL.Installers;

public class DALInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        // Get the assembly where the repositories are located
        var repositoryAssembly = typeof(RepositoryBase<>).Assembly;

        // Find all types that inherit from RepositoryBase<>
        var repositoryTypes = repositoryAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(RepositoryBase<>))
            .ToList();

        foreach (var repositoryType in repositoryTypes)
        {// Find the corresponding repository interface (direct or indirect)
            var repositoryInterface = repositoryType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IRepositoryBase<>) && 
                                     i.GetInterfaces().Any(baseInterface => baseInterface.IsGenericType &&
                                                                            baseInterface.GetGenericTypeDefinition() == typeof(IRepositoryBase<>)));

            if (repositoryInterface != null)
            {
                serviceCollection.AddScoped(repositoryInterface, repositoryType);
            }
            else
            {
                Console.WriteLine($"No matching interface found for {repositoryType.FullName}");
            }
        }

    }
}