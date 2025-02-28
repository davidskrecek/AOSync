using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;


public class TimesheetRepository : RepositoryBase<TimesheetEntity>, ITimesheetService
{
    public TimesheetRepository(AOSyncDbContext context) : base(context)
    {
    }
}