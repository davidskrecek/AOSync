using AOSync.DAL.Entities;
using AOSync.BL.Facades.Interfaces;
using AOSync.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AOSync.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AOSync.BL.Facades
{
    public class FacadeBase<T> : IFacadeBase<T> where T : EntityBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepositoryBase<T> _repository;

        public FacadeBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _repository = _serviceProvider.GetRequiredService<IRepositoryBase<T>>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}