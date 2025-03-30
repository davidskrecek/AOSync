using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class ChangeTrackingRepository : RepositoryBase<EntityBase>, IChangeTrackingRepository
{
    public ChangeTrackingRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public async Task<ICollection<EntityBase>> GetCreatedRecordsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var createdRecords = new List<EntityBase>();

        createdRecords.AddRange(await context.Projects.Where(p => p.IsCreated == true)
            .ToListAsync());
        createdRecords.AddRange(await context.Sections.Where(s => s.IsCreated == true)
            .ToListAsync());
        createdRecords.AddRange(await context.Tasks.Where(t => t.IsCreated == true)
            .ToListAsync());
        createdRecords.AddRange(await context.Comments.Where(c => c.IsCreated == true)
            .ToListAsync());

        return createdRecords;
    }

    public async Task<ICollection<EntityBase>> GetChangedRecordsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var changedRecords = new List<EntityBase>();

        changedRecords.AddRange(await context.Projects.Where(p => p.IsChanged)
            .ToListAsync());
        changedRecords.AddRange(await context.Sections.Where(s => s.IsChanged)
            .ToListAsync());
        changedRecords.AddRange(await context.Tasks.Where(t => t.IsChanged).ToListAsync());
        changedRecords.AddRange(await context.Comments.Where(t => t.IsChanged)
            .ToListAsync());

        return changedRecords;
    }

    public async Task<ICollection<EntityBase>> GetDeletedRecordsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var deletedRecords = new List<EntityBase>();

        deletedRecords.AddRange(await context.Projects.Where(p => p.IsDeleted)
            .ToListAsync());
        deletedRecords.AddRange(await context.Sections.Where(s => s.IsDeleted)
            .ToListAsync());
        deletedRecords.AddRange(await context.Tasks.Where(t => t.IsDeleted).ToListAsync());
        deletedRecords.AddRange(await context.Comments.Where(t => t.IsDeleted)
            .ToListAsync());

        return deletedRecords;
    }

    public async Task ResetFlags(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var entity =
            await context.Projects.FirstOrDefaultAsync(p => p.Id == id) as EntityBase ??
            await context.Tasks.FirstOrDefaultAsync(t => t.Id == id) as EntityBase ??
            await context.Comments.FirstOrDefaultAsync(c => c.Id == id) as EntityBase ??
            await context.Timesheets.FirstOrDefaultAsync(ts => ts.Id == id);

        if (entity != null)
        {
            entity.IsChanged = entity.IsDeleted = entity.IsCreated = false;
            await context.SaveChangesAsync();
        }
    }
}