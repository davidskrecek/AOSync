using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class WorkspaceRepository : RepositoryBase<WorkspaceEntity>, IWorkspaceRepository
{
    public WorkspaceRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
        
    }
}