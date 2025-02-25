using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.DB
{
    public class AOSyncDbContext : DbContext
    {
        public AOSyncDbContext(DbContextOptions<AOSyncDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserGroupEntity> UserGroups { get; set; }
        public DbSet<SolverGroupEntity> SolverGroups { get; set; }
        public DbSet<WorkspaceEntity> Workspaces { get; set; }
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<PredefinedCommentEntity> PredefinedComments { get; set; }
        public DbSet<AttributeEntity> Attributes { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<SectionEntity> Sections { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }
        public DbSet<TimesheetEntity> Timesheets { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Many-to-Many: Projects ↔ Users (Members)
            modelBuilder.Entity<ProjectEntity>()
                .HasMany(p => p.Members)
                .WithMany(u => u.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectUser",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<ProjectEntity>().WithMany().HasForeignKey("ProjectId")
                );

            // ✅ Many-to-Many: Teams ↔ Users (Members)
            modelBuilder.Entity<TeamEntity>()
                .HasMany(t => t.Members)
                .WithMany(u => u.Teams)
                .UsingEntity<Dictionary<string, object>>(
                    "TeamMembership",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<TeamEntity>().WithMany().HasForeignKey("TeamId")
                );

            // ✅ Many-to-Many: UserGroups ↔ Users (Members)
            modelBuilder.Entity<UserGroupEntity>()
                .HasMany(ug => ug.Members)
                .WithMany(u => u.UserGroups)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGroupMembership",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<UserGroupEntity>().WithMany().HasForeignKey("UserGroupId")
                );

            // ✅ Many-to-Many: SolverGroups ↔ Users (Members)
            modelBuilder.Entity<SolverGroupEntity>()
                .HasMany(sg => sg.Members)
                .WithMany(u => u.SolverGroups)
                .UsingEntity<Dictionary<string, object>>(
                    "SolverGroupMembership",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<SolverGroupEntity>().WithMany().HasForeignKey("SolverGroupId")
                );

            // ✅ Self-referencing Parent-Child relationship for Tasks
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.ParentTask)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Many-to-Many: Tasks ↔ Users (Solvers)
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.Solvers)
                .WithMany(u => u.SolvedTasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskSolver",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("SolverId"),
                    j => j.HasOne<TaskEntity>().WithMany().HasForeignKey("TaskId")
                );

            // ✅ One-to-Many: Task ↔ Owner (User)
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.OwnedTasks)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ✅ Many-to-Many: Tasks ↔ Users (Followers)
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.Followers)
                .WithMany(u => u.FollowedTasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskFollower",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("FollowerId"),
                    j => j.HasOne<TaskEntity>().WithMany().HasForeignKey("TaskId")
                );

            // ✅ One-to-Many: Comments ↔ Users (CreatedBy)
            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ One-to-Many: Timesheets ↔ Task, Project, Team
            modelBuilder.Entity<TimesheetEntity>()
                .HasOne(ts => ts.Task)
                .WithMany(t => t.Timesheets)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimesheetEntity>()
                .HasOne(ts => ts.Project)
                .WithMany(p => p.Timesheets)
                .HasForeignKey(ts => ts.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimesheetEntity>()
                .HasOne(ts => ts.Team)
                .WithMany(t => t.Timesheets)
                .HasForeignKey(ts => ts.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ One-to-Many: Attachments ↔ Task, Project, Team, Comment
            modelBuilder.Entity<AttachmentEntity>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttachmentEntity>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Attachments)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttachmentEntity>()
                .HasOne(a => a.Team)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttachmentEntity>()
                .HasOne(a => a.Comment)
                .WithMany(c => c.Attachments)
                .HasForeignKey(a => a.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
