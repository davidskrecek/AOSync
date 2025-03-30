using AOSync.BL.Facades.Interfaces;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.BL.Facades;

public class ProjectFacade : FacadeBase<ProjectEntity>, IProjectFacade
{
    public ProjectFacade(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        
    }
}