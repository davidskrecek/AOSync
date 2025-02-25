﻿// <auto-generated />
using System;
using AOSync.DAL.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AOSync.DAL.Migrations
{
    [DbContext(typeof(AOSyncDbContext))]
    [Migration("20250221125123_2025-02-21-4-Fixed-Task-Owner")]
    partial class _202502214FixedTaskOwner
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("AOSync.DAL.DB.AttachmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("CommentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("FileContent")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskId");

                    b.HasIndex("TeamId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("AOSync.DAL.DB.AttributeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("WorkspaceId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("AOSync.DAL.DB.CommentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("TaskId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("AOSync.DAL.DB.PredefinedCommentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("PredefinedComments");
                });

            modelBuilder.Entity("AOSync.DAL.DB.ProjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("AOSync.DAL.DB.SectionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TeamId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("AOSync.DAL.DB.SolverGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SolverGroups");
                });

            modelBuilder.Entity("AOSync.DAL.DB.StageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ParentTaskId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ParentTaskId");

                    b.HasIndex("SectionId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TeamEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TimesheetEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Minutes")
                        .HasColumnType("double");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("char(36)");

                    b.Property<string>("SourceComponent")
                        .IsRequired()
                        .HasMaxLength(38)
                        .HasColumnType("varchar(38)");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskId");

                    b.HasIndex("TeamId");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AOSync.DAL.DB.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMentored")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AOSync.DAL.DB.UserGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("AOSync.DAL.DB.WorkspaceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Archived")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChanged")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUser");
                });

            modelBuilder.Entity("SolverGroupMembership", b =>
                {
                    b.Property<Guid>("SolverGroupId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("SolverGroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("SolverGroupMembership");
                });

            modelBuilder.Entity("TaskFollower", b =>
                {
                    b.Property<Guid>("FollowerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("char(36)");

                    b.HasKey("FollowerId", "TaskId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskFollower");
                });

            modelBuilder.Entity("TaskSolver", b =>
                {
                    b.Property<Guid>("SolverId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("char(36)");

                    b.HasKey("SolverId", "TaskId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskSolver");
                });

            modelBuilder.Entity("TeamMembership", b =>
                {
                    b.Property<Guid>("TeamId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("TeamId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TeamMembership");
                });

            modelBuilder.Entity("UserGroupMembership", b =>
                {
                    b.Property<Guid>("UserGroupId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserGroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroupMembership");
                });

            modelBuilder.Entity("AOSync.DAL.DB.AttachmentEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.CommentEntity", "Comment")
                        .WithMany("Attachments")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AOSync.DAL.DB.ProjectEntity", "Project")
                        .WithMany("Attachments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AOSync.DAL.DB.TaskEntity", "Task")
                        .WithMany("Attachments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AOSync.DAL.DB.TeamEntity", "Team")
                        .WithMany("Attachments")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Comment");

                    b.Navigation("Project");

                    b.Navigation("Task");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("AOSync.DAL.DB.AttributeEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.WorkspaceEntity", "Workspace")
                        .WithMany("Attributes")
                        .HasForeignKey("WorkspaceId");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("AOSync.DAL.DB.CommentEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.UserEntity", "CreatedByUser")
                        .WithMany("Comments")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.TaskEntity", "Task")
                        .WithMany("Comments")
                        .HasForeignKey("TaskId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("AOSync.DAL.DB.PredefinedCommentEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.WorkspaceEntity", "Workspace")
                        .WithMany("PredefinedComments")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("AOSync.DAL.DB.SectionEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.ProjectEntity", "Project")
                        .WithMany("Sections")
                        .HasForeignKey("ProjectId");

                    b.HasOne("AOSync.DAL.DB.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.Navigation("Project");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("AOSync.DAL.DB.StageEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.WorkspaceEntity", "Workspace")
                        .WithMany("Stages")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TaskEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.UserEntity", "Owner")
                        .WithMany("OwnedTasks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.TaskEntity", "ParentTask")
                        .WithMany("Subtasks")
                        .HasForeignKey("ParentTaskId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AOSync.DAL.DB.SectionEntity", "Section")
                        .WithMany("Tasks")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("ParentTask");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TimesheetEntity", b =>
                {
                    b.HasOne("AOSync.DAL.DB.ProjectEntity", "Project")
                        .WithMany("Timesheets")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AOSync.DAL.DB.TaskEntity", "Task")
                        .WithMany("Timesheets")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AOSync.DAL.DB.TeamEntity", "Team")
                        .WithMany("Timesheets")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Project");

                    b.Navigation("Task");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.HasOne("AOSync.DAL.DB.ProjectEntity", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SolverGroupMembership", b =>
                {
                    b.HasOne("AOSync.DAL.DB.SolverGroupEntity", null)
                        .WithMany()
                        .HasForeignKey("SolverGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskFollower", b =>
                {
                    b.HasOne("AOSync.DAL.DB.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.TaskEntity", null)
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskSolver", b =>
                {
                    b.HasOne("AOSync.DAL.DB.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("SolverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.TaskEntity", null)
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamMembership", b =>
                {
                    b.HasOne("AOSync.DAL.DB.TeamEntity", null)
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserGroupMembership", b =>
                {
                    b.HasOne("AOSync.DAL.DB.UserGroupEntity", null)
                        .WithMany()
                        .HasForeignKey("UserGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AOSync.DAL.DB.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AOSync.DAL.DB.CommentEntity", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("AOSync.DAL.DB.ProjectEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Sections");

                    b.Navigation("Timesheets");
                });

            modelBuilder.Entity("AOSync.DAL.DB.SectionEntity", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TaskEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Comments");

                    b.Navigation("Subtasks");

                    b.Navigation("Timesheets");
                });

            modelBuilder.Entity("AOSync.DAL.DB.TeamEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Timesheets");
                });

            modelBuilder.Entity("AOSync.DAL.DB.UserEntity", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("OwnedTasks");
                });

            modelBuilder.Entity("AOSync.DAL.DB.WorkspaceEntity", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("PredefinedComments");

                    b.Navigation("Stages");
                });
#pragma warning restore 612, 618
        }
    }
}
