using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;

public interface IChangeTrackingRepository : IRepositoryBase<EntityBase>
{
    Task<ICollection<EntityBase>> GetChangedRecordsAsync();
    Task<ICollection<EntityBase>> GetCreatedRecordsAsync();
    Task<ICollection<EntityBase>> GetDeletedRecordsAsync();
    Task ResetFlags(Guid id);
}