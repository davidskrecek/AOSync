using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.Entities;

public class SectionEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string ExternalId { get; set; }

    // Foreign keys to represent polymorphic relationships
    [ForeignKey("Project")]
    public Guid? ProjectId { get; set; }
    public ProjectEntity Project { get; set; }

    [ForeignKey("Team")]
    public Guid? TeamId { get; set; }
    public TeamEntity Team { get; set; }

    // Navigation property for Tasks
    public ICollection<TaskEntity> Tasks { get; set; }
}