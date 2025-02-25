using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.DB;

public class SectionService : DataService<SectionEntity>, ISectionService
{
    public SectionService(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<ICollection<SectionEntity>> GetSectionsByProjectId(Guid projectId)
    {
        return await _context.Sections
            .Where(s => s.ProjectId == projectId)
            .ToListAsync();
    }
}