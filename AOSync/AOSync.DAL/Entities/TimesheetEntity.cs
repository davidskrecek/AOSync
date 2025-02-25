using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.DB;

public class TimesheetEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string? ExternalId { get; set; }

    [Required]
    public DateTime Date { get; set; } // Date of the timesheet entry

    [Required]
    public double Minutes { get; set; } // Hours logged in the timesheet

    public string? Description { get; set; }

    [MaxLength(38)]
    [MinLength(5)]
    public string SourceComponent { get; set; } // Component ID, matches the API description

    // Nullable foreign keys to link to related entities
    [ForeignKey("Task")]
    public Guid? TaskId { get; set; }
    public TaskEntity Task { get; set; }

    [ForeignKey("Project")]
    public Guid? ProjectId { get; set; }
    public ProjectEntity Project { get; set; }

    [ForeignKey("Team")]
    public Guid? TeamId { get; set; }
    public TeamEntity Team { get; set; }
}
