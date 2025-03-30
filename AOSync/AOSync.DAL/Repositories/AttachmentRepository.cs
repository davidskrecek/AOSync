using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class AttachmentRepository : RepositoryBase<AttachmentEntity>, IAttachmentRepository
{
    public AttachmentRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public async Task<ICollection<AttachmentEntity>> GetAttachmentsByTaskId(Guid taskId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Attachments
            .Where(a => a.TaskId == taskId)
            .ToListAsync();
    }
}