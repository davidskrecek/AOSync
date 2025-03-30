using AOSync.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AOSync.BL.Facades.Interfaces
{
    public interface IFacadeBase<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
    }
}