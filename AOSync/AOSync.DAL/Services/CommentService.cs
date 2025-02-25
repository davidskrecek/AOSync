using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.DB;


public class CommentService : DataService<CommentEntity>, ICommentService
{
    public CommentService(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<CommentEntity>> GetCommentsByTaskId(Guid taskId)
    {
        return await _context.Comments
            .Where(c => c.TaskId == taskId)
            .ToListAsync();
    }
}