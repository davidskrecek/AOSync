using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;


public class TimesheetRepository : RepositoryBase<TimesheetEntity>, ITimesheetRepository
{
    public TimesheetRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }
}