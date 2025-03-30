using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class TeamRepository : RepositoryBase<TeamEntity>, ITeamRepository
{
    public TeamRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }
}