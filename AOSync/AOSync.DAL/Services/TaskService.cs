using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.DB;


public class TaskService : DataService<TaskEntity>, ITaskService
{
    public TaskService(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<TaskEntity>> GetTasksBySectionId(Guid sectionId)
    {
        return await _context.Tasks
            .Where(t => t.SectionId == sectionId)
            .ToListAsync();
    }
}