using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class AttachmentRepository : RepositoryBase<AttachmentEntity>, IAttachmentRepository
{
    public AttachmentRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<AttachmentEntity>> GetAttachmentsByTaskId(Guid taskId)
    {
        return await _context.Attachments
            .Where(a => a.TaskId == taskId)
            .ToListAsync();
    }
}