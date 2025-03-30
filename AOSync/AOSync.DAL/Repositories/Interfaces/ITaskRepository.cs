using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface ITaskRepository : IRepositoryBase<TaskEntity>
{
    Task<ICollection<TaskEntity>> GetTasksBySectionId(Guid sectionId);
}