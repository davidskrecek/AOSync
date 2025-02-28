using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface IAttachmentRepository : IRepositoryBase<AttachmentEntity>
{
    Task<ICollection<AttachmentEntity>> GetAttachmentsByTaskId(Guid taskId);
}