using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class SolverGroupRepository : RepositoryBase<SolverGroupEntity>, ISolverGroupRepository
{
    public SolverGroupRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
        
    }
}