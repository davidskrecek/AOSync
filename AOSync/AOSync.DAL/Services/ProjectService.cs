namespace AOSync.DAL.DB;

public class ProjectService : DataService<ProjectEntity>, IProjectService
{
    public ProjectService(AOSyncDbContext context) : base(context)
    {
    }
}