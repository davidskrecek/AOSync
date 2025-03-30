using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class StageRepository : RepositoryBase<StageEntity>, IStageRepository
{
    public StageRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
        
    }
}