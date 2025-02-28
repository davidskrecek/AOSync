using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class TeamRepository : RepositoryBase<TeamEntity>, ITeamRepository
{
    public TeamRepository(AOSyncDbContext context) : base(context)
    {
    }
}