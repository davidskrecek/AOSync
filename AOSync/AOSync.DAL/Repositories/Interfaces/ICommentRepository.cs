using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface ICommentRepository : IRepositoryBase<CommentEntity>
{
    Task<ICollection<CommentEntity>> GetCommentsByTaskId(Guid taskId);
}