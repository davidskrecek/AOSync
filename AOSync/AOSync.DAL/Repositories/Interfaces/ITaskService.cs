using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface ITaskService : IRepositoryBase<TaskEntity>
{
    Task<ICollection<TaskEntity>> GetTasksBySectionId(Guid sectionId);
}