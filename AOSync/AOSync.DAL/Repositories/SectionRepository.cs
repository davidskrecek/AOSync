using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class SectionRepository : RepositoryBase<SectionEntity>, ISectionRepository
{
    public SectionRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public async Task<ICollection<SectionEntity>> GetSectionsByProjectId(Guid projectId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Sections
            .Where(s => s.ProjectId == projectId)
            .ToListAsync();
    }
}