using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;


public class CommentRepository : RepositoryBase<CommentEntity>, ICommentRepository
{
    public CommentRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public async Task<ICollection<CommentEntity>> GetCommentsByTaskId(Guid taskId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Comments
            .Where(c => c.TaskId == taskId)
            .ToListAsync();
    }
}