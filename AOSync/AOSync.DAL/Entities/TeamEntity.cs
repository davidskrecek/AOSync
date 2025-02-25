using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.DB;

public class TeamEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }

    public ICollection<UserEntity> Members { get; set; }
    public ICollection<TimesheetEntity> Timesheets { get; set; }
    public ICollection<AttachmentEntity> Attachments { get; set; } // New
}