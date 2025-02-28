using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.Entities;

public class UserEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsMentored { get; set; }

    // Navigation properties
    public ICollection<UserGroupEntity> UserGroups { get; set; }
    public ICollection<SolverGroupEntity> SolverGroups { get; set; }
    public ICollection<TeamEntity> Teams { get; set; }
    public ICollection<ProjectEntity> Projects { get; set; } // New
    public ICollection<TaskEntity> OwnedTasks { get; set; } = new List<TaskEntity>();
    public ICollection<TaskEntity> FollowedTasks { get; set; } = new List<TaskEntity>();
    public ICollection<TaskEntity> SolvedTasks { get; set; } = new List<TaskEntity>();
    public ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

}