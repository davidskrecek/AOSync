using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.Entities;

public class PredefinedCommentEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string Text { get; set; }

    [ForeignKey("Workspace")]
    public Guid WorkspaceId { get; set; }
    public WorkspaceEntity Workspace { get; set; }
}