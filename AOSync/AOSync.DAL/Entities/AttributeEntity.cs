using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOSync.DAL.Entities;

public record AttributeEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }

    [ForeignKey("Workspace")]
    public Guid? WorkspaceId { get; set; }
    public WorkspaceEntity Workspace { get; set; }
}