using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;

public interface ISectionService : IRepositoryBase<SectionEntity>
{
    Task<ICollection<SectionEntity>> GetSectionsByProjectId(Guid projectId);
}