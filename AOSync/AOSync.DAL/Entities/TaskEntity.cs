using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.DB;

public class TaskEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }

    [ForeignKey("Section")]
    public Guid SectionId { get; set; }
    public SectionEntity Section { get; set; }

    [ForeignKey("ParentTask")]
    public Guid? ParentTaskId { get; set; }
    public TaskEntity ParentTask { get; set; }
    public ICollection<TaskEntity> Subtasks { get; set; }

    public ICollection<CommentEntity> Comments { get; set; }
    
    [ForeignKey("OwnerId")]
    public Guid OwnerId { get; set; }  // ✅ Ensure only one Foreign Key
    public UserEntity Owner { get; set; }  // ✅ Ensure only one navigation property
    
    // Many-to-Many: Task <-> Users (Solvers)
    public ICollection<UserEntity> Solvers { get; set; } = new List<UserEntity>();
    public ICollection<UserEntity> Followers { get; set; } = new List<UserEntity>();
    public ICollection<TimesheetEntity> Timesheets { get; set; } = new List<TimesheetEntity>();
    public ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();

}
