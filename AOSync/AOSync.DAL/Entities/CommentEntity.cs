using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.Entities;

public record CommentEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string? ExternalId { get; set; }
    public string Text { get; set; }

    // Foreign Key for the user who created the comment
    [ForeignKey("CreatedByUser")]
    public Guid CreatedByUserId { get; set; }  
    public UserEntity CreatedByUser { get; set; }

    // Optional Foreign Key for Task (if comments belong to tasks)
    [ForeignKey("Task")]
    public Guid? TaskId { get; set; }  
    public TaskEntity? Task { get; set; }
    
    public ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
}