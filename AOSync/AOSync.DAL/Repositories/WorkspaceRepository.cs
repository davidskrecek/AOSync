using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class WorkspaceRepository : RepositoryBase<WorkspaceEntity>, IWorkspaceRepository
{
    public WorkspaceRepository(AOSyncDbContext context) : base(context)
    {
        
    }
}