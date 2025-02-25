using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.DB;

public class AttachmentEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string? ExternalId { get; set; }
    public string FileName { get; set; }
    public byte[] FileContent { get; set; } // Binary content storage

    // Foreign keys for polymorphic attachment usage
    [ForeignKey("Task")]
    public Guid? TaskId { get; set; }
    public TaskEntity Task { get; set; }

    [ForeignKey("Project")]
    public Guid? ProjectId { get; set; }
    public ProjectEntity Project { get; set; }

    [ForeignKey("Team")]
    public Guid? TeamId { get; set; }
    public TeamEntity Team { get; set; }

    [ForeignKey("Comment")]
    public Guid? CommentId { get; set; }
    public CommentEntity Comment { get; set; }
}
