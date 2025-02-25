namespace AOSync.DAL.DB;


public class TimesheetService : DataService<TimesheetEntity>, ITimesheetService
{
    public TimesheetService(AOSyncDbContext context) : base(context)
    {
    }
}