using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class SectionRepository : RepositoryBase<SectionEntity>, ISectionService
{
    public SectionRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<SectionEntity>> GetSectionsByProjectId(Guid projectId)
    {
        return await _context.Sections
            .Where(s => s.ProjectId == projectId)
            .ToListAsync();
    }
}