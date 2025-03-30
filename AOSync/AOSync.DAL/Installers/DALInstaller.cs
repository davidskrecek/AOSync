using AOSync.COMMON.Installers;
using AOSync.DAL.Repositories;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using AOSync.DAL.Entities;

namespace AOSync.DAL.Installers
{
    public class DALInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            // Register repositories with their concrete implementations
            RegisterRepositories(serviceCollection);
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            // Register IRepositoryBase with its concrete implementation
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
        }
    }
}