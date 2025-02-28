using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;


public class CommentRepository : RepositoryBase<CommentEntity>, ICommentRepository
{
    public CommentRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<CommentEntity>> GetCommentsByTaskId(Guid taskId)
    {
        return await _context.Comments
            .Where(c => c.TaskId == taskId)
            .ToListAsync();
    }
}