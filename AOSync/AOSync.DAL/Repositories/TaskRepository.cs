using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;


public class TaskRepository : RepositoryBase<TaskEntity>, ITaskRepository
{
    public TaskRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public async Task<ICollection<TaskEntity>> GetTasksBySectionId(Guid sectionId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Tasks
            .Where(t => t.SectionId == sectionId)
            .ToListAsync();
    }
}