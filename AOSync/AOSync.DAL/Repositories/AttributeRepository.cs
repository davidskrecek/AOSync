using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class AttributeRepository : RepositoryBase<AttributeEntity>, IAttributeRepository
{
    public AttributeRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<AttributeEntity>> GetAttributesByTaskId(Guid taskId)
    {
        return new List<AttributeEntity>();
    }
}