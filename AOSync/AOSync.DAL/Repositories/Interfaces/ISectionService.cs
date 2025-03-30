using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;

public interface ISectionRepository : IRepositoryBase<SectionEntity>
{
    Task<ICollection<SectionEntity>> GetSectionsByProjectId(Guid projectId);
}