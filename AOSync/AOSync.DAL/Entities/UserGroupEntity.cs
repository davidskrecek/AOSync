using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.DB;

public class UserGroupEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }

    // Navigation properties
    public ICollection<UserEntity> Members { get; set; }
}