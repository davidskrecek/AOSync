using AOSync.BL.Services.Synchronization.Interfaces;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.BL.Services.Synchronization;

public class ProjectService : ServiceBase<ProjectEntity>, IProjectService
{
    public ProjectService(IProjectRepository projectRepository) 
        : base(projectRepository) // Pass the repository to the base class
    {
    }
}