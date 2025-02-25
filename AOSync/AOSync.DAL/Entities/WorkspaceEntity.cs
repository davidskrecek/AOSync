using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.DB;

public class WorkspaceEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }

    // Navigation properties
    public ICollection<StageEntity> Stages { get; set; }
    public ICollection<PredefinedCommentEntity> PredefinedComments { get; set; }
    public ICollection<AttributeEntity> Attributes { get; set; }
}