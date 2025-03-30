using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class ProjectRepository : RepositoryBase<ProjectEntity>, IProjectRepository
{
    public ProjectRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }
}