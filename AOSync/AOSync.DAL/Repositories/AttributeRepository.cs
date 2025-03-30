using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class AttributeRepository : RepositoryBase<AttributeEntity>, IAttributeRepository
{
    public AttributeRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public async Task<ICollection<AttributeEntity>> GetAttributesByTaskId(Guid taskId)
    {
        return new List<AttributeEntity>();
    }
}