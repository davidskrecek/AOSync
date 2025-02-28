using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class StageRepository : RepositoryBase<StageEntity>, IStageRepository
{
    public StageRepository(AOSyncDbContext context) : base(context)
    {
        
    }
}