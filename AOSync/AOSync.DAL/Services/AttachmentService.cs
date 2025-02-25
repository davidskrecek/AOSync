using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.DB;

public class AttachmentService : DataService<AttachmentEntity>, IAttachmentService
{
    public AttachmentService(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<AttachmentEntity>> GetAttachmentsByTaskId(Guid taskId)
    {
        return await _context.Attachments
            .Where(a => a.TaskId == taskId)
            .ToListAsync();
    }
}