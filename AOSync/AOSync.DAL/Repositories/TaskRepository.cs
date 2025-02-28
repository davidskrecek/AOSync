using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;


public class TaskRepository : RepositoryBase<TaskEntity>, ITaskService
{
    public TaskRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<TaskEntity>> GetTasksBySectionId(Guid sectionId)
    {
        return await _context.Tasks
            .Where(t => t.SectionId == sectionId)
            .ToListAsync();
    }
}