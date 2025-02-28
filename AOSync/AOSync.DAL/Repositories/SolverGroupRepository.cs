using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class SolverGroupRepository : RepositoryBase<SolverGroupEntity>, ISolverGroupRepository
{
    public SolverGroupRepository(AOSyncDbContext context) : base(context)
    {
        
    }
}