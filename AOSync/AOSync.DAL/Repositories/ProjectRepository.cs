using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class ProjectRepository : RepositoryBase<ProjectEntity>, IProjectRepository
{
    public ProjectRepository(AOSyncDbContext context) : base(context)
    {
    }
}