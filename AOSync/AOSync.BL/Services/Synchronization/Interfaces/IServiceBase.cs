using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AOSync.BL.Services.Synchronization.Interfaces
{
    public interface IServiceBase<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByEIdAsync(string eid);
        Task<Guid> GetIdByEIdAsync(string eid);
        Task<T> AddAsync(T entity);
        Task<T> AddOrUpdateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> AddEId(Guid id, string eid);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByEIdAsync(string eid);
        Task AddEIdByIdAsync(Guid id, string eid);
    }
}