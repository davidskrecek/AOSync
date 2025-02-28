using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class ChangeTrackingRepository : RepositoryBase<EntityBase>, IChangeTrackingRepository
{
    public ChangeTrackingRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<EntityBase>> GetCreatedRecordsAsync()
    {
        var createdRecords = new List<EntityBase>();

        createdRecords.AddRange(await _context.Projects.Where(p => p.IsCreated == true)
            .ToListAsync());
        createdRecords.AddRange(await _context.Sections.Where(s => s.IsCreated == true)
            .ToListAsync());
        createdRecords.AddRange(await _context.Tasks.Where(t => t.IsCreated == true)
            .ToListAsync());
        createdRecords.AddRange(await _context.Comments.Where(c => c.IsCreated == true)
            .ToListAsync());

        return createdRecords;
    }

    public async Task<ICollection<EntityBase>> GetChangedRecordsAsync()
    {
        var changedRecords = new List<EntityBase>();

        changedRecords.AddRange(await _context.Projects.Where(p => p.IsChanged)
            .ToListAsync());
        changedRecords.AddRange(await _context.Sections.Where(s => s.IsChanged)
            .ToListAsync());
        changedRecords.AddRange(await _context.Tasks.Where(t => t.IsChanged).ToListAsync());
        changedRecords.AddRange(await _context.Comments.Where(t => t.IsChanged)
            .ToListAsync());

        return changedRecords;
    }

    public async Task<ICollection<EntityBase>> GetDeletedRecordsAsync()
    {
        var deletedRecords = new List<EntityBase>();

        deletedRecords.AddRange(await _context.Projects.Where(p => p.IsDeleted)
            .ToListAsync());
        deletedRecords.AddRange(await _context.Sections.Where(s => s.IsDeleted)
            .ToListAsync());
        deletedRecords.AddRange(await _context.Tasks.Where(t => t.IsDeleted).ToListAsync());
        deletedRecords.AddRange(await _context.Comments.Where(t => t.IsDeleted)
            .ToListAsync());

        return deletedRecords;
    }

    public async Task ResetFlags(Guid id)
    {
        var entity =
            await _context.Projects.FirstOrDefaultAsync(p => p.Id == id) as EntityBase ??
            await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id) as EntityBase ??
            await _context.Comments.FirstOrDefaultAsync(c => c.Id == id) as EntityBase ??
            await _context.Timesheets.FirstOrDefaultAsync(ts => ts.Id == id);

        if (entity != null)
        {
            entity.IsChanged = entity.IsDeleted = entity.IsCreated = false;
            await _context.SaveChangesAsync();
        }
    }
}