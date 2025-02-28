using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;

public interface IAttributeRepository : IRepositoryBase<AttributeEntity>
{
    Task<ICollection<AttributeEntity>> GetAttributesByTaskId(Guid taskId);
}