using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AOSync.BL.Services.Synchronization.Interfaces;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.BL.Services.Synchronization
{
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        protected readonly IRepositoryBase<T> _repository;

        public ServiceBase(IRepositoryBase<T> repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T?> GetByEIdAsync(string eid)
        {
            return await _repository.GetByEIdAsync(eid);
        }

        public async Task<Guid> GetIdByEIdAsync(string eid)
        {
            return await _repository.GetIdByEIdAsync(eid);
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<T> AddOrUpdateAsync(T entity)
        {
            return await _repository.AddOrUpdateAsync(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> AddEId(Guid id, string eid)
        {
            return await _repository.AddEId(id, eid);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByEIdAsync(string eid)
        {
            return await _repository.ExistsByEIdAsync(eid);
        }

        public async Task AddEIdByIdAsync(Guid id, string eid)
        {
            await _repository.AddEIdByIdAsync(id, eid);
        }
    }
}