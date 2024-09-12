using System.Diagnostics;
using AOSync.Model;
using Microsoft.Extensions.DependencyInjection;
using AOSync.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AOSync.Converters;
using AOSync.Services;

namespace AOSync
{
    public class Program
    {
        public string? LastTransId { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ConsoleExtension _consoleExtension;
        private readonly ApiCommunicator _apiCommunicator;
        private readonly DataReloadService _dataReloadService;
        private bool _dataChanged;
        private List<syncExternal> _externals;

        public Program(IConfiguration configuration, IServiceProvider serviceProvider, DataReloadService dataReloadService)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _dataReloadService = dataReloadService;

            _consoleExtension = new ConsoleExtension(_serviceProvider);
            _apiCommunicator = new ApiCommunicator(this, _serviceProvider);
            
            _dataChanged = false;

            _externals = new List<syncExternal>();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("AOSyncBetaDB"),
                    new MySqlServerVersion(new Version(8, 0, 21))
                )
            );

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDataService<ComponentEntity>, ComponentService>();
            services.AddScoped<ITimeSheetService, TimeSheetService>();
            services.AddScoped<IChangeTrackingService, ChangeTrackingService>();

            services.AddSingleton<DataReloadService>();

            services.AddTransient<Program>();
            services.AddTransient<ApiCommunicator>();

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new ComponentBaseConverter());
                        options.SerializerSettings.Converters.Add(new DateTimeConverter());
                    });
        }

        public async Task HandleComponents(List<ComponentBase> components)
        {
            foreach (ComponentBase component in components)
            {
                using var scope = _serviceProvider.CreateScope();

                switch (component)
                {
                    case syncProject project:
                        await HandleProjectComponent(project, scope);
                        _dataChanged = true;
                        break;
                    case syncSection section:
                        await HandleSectionComponent(section, scope);
                        _dataChanged = true;
                        break;
                    case syncTask task:
                        await HandleTaskComponent(task, scope);
                        _dataChanged = true;
                        break;
                    case syncAttachment attachment:
                        await HandleAttachmentComponent(attachment, scope);
                        _dataChanged = true;
                        break;
                    case syncUserCompany user:
                        await HandleUserComponent(user, scope);
                        _dataChanged = true;
                        break;
                    case syncComment comment:
                        await HandleCommentComponent(comment, scope);
                        _dataChanged = true;
                        break;
                    case syncTimeSheet timeSheet:
                        await HandleTimesheetComponent(timeSheet, scope);
                        _dataChanged = true;
                        break;
                    default:
                        continue;
                }
            }
            
            if (_dataChanged)
            {
                await _apiCommunicator.SyncSetExternals(_externals);
                _externals.Clear();
                _dataReloadService.NotifyDataChanged();
                _dataChanged = false;
            }
        }

        private async Task HandleProjectComponent(syncProject project, IServiceScope scope)
        {
            var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
            var existingProjectEntity = await projectService.GetByEIdAsync(project.id!);

            var projectEntity = existingProjectEntity ?? new ProjectEntity
            {
                Id = project.eid != null ? new Guid(project.eid) : Guid.NewGuid(),
                ExternalId = project.id ?? Guid.NewGuid().ToString()
            };

            projectEntity.Name = project.Name ?? existingProjectEntity?.Name ?? string.Empty;
            projectEntity.Archived = project.archived ?? existingProjectEntity?.Archived;

            switch (project.type)
            {
                case "Create":
                    await projectService.AddOrUpdateAsync(projectEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = projectEntity.Id.ToString(),
                        id = projectEntity.ExternalId!
                    });
                    break;
                case "Modify":
                    await projectService.AddOrUpdateAsync(projectEntity);
                    break;
                case "Delete":
                    await projectService.DeleteAsync(new Guid(project.eid!));
                    break;
            }
        }

        private async Task HandleSectionComponent(syncSection section, IServiceScope scope)
        {
            var sectionService = scope.ServiceProvider.GetRequiredService<ISectionService>();
            var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();

            var existingSectionEntity = await sectionService.GetByEIdAsync(section.id!);
            var sectionEntity = existingSectionEntity ?? new SectionEntity
            {
                Id = section.eid != null ? new Guid(section.eid) : Guid.NewGuid(),
                ExternalId = section.id ?? Guid.NewGuid().ToString()
            };

            if (section.Name != null)
            {
                sectionEntity.Name = section.Name;
            }

            if (section.parent?.id != null)
            {
                if (await projectService.ExistsByEIdAsync(section.parent.id))
                {
                    sectionEntity.ProjectId = await projectService.GetIdByEIdAsync(section.parent.id);
                }
                else
                {
                    throw new Exception("PROJECT NENÍ");
                }
            }
            else if (existingSectionEntity != null)
            {
                sectionEntity.ProjectId = existingSectionEntity.ProjectId;
            }

            switch (section.type)
            {
                case "Create":
                    await sectionService.AddOrUpdateAsync(sectionEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = sectionEntity.Id.ToString(),
                        id = sectionEntity.ExternalId!
                    });
                    break;
                case "Modify":
                    await sectionService.AddOrUpdateAsync(sectionEntity);
                    break;
                case "Delete":
                    await sectionService.DeleteAsync(new Guid(section.eid!));
                    break;
            }
        }

        private async Task HandleTaskComponent(syncTask task, IServiceScope scope)
        {
            var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
            var sectionService = scope.ServiceProvider.GetRequiredService<ISectionService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var existingTaskEntity = await taskService.GetByEIdAsync(task.id!);
            var taskEntity = existingTaskEntity ?? new TaskEntity
            {
                Id = task.eid != null ? new Guid(task.eid) : Guid.NewGuid(),
                ExternalId = task.id ?? Guid.NewGuid().ToString()
            };

            taskEntity.Name = task.Name ?? existingTaskEntity?.Name ?? string.Empty;
            taskEntity.Archived = task.archived ?? existingTaskEntity?.Archived;

            if (task.parent?.id != null)
            {
                if (await sectionService.ExistsByEIdAsync(task.parent?.id!))
                {
                    taskEntity.SectionId = await sectionService.GetIdByEIdAsync(task.parent?.id!);
                }
                else
                {
                    throw new Exception("SECTION NENÍ");
                }
            }
            else if (existingTaskEntity != null)
            {
                taskEntity.SectionId = existingTaskEntity.SectionId;
            }

            if (task.Owner != null)
            {
                if (await userService.ExistsByEIdAsync(task.Owner))
                {
                    taskEntity.OwnerId = await userService.GetIdByEIdAsync(task.Owner);
                    
                }
                else
                {
                    throw new Exception("USER NENÍ");
                }
            }
            else if (existingTaskEntity != null)
            {
                taskEntity.OwnerId = existingTaskEntity.OwnerId;
            }

            switch (task.type)
            {
                case "Create":
                    await taskService.AddOrUpdateAsync(taskEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = taskEntity.Id.ToString(),
                        id = taskEntity.ExternalId!
                    });
                    break;
                case "Modify":
                    await taskService.AddOrUpdateAsync(taskEntity);
                    break;
                case "Delete":
                    await taskService.DeleteAsync(new Guid(task.eid!));
                    break;
            }
        }

        private async Task HandleAttachmentComponent(syncAttachment attachment, IServiceScope scope)
        {
            var attachmentService = scope.ServiceProvider.GetRequiredService<IAttachmentService>();
            var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
            var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

            var existingAttachmentEntity = await attachmentService.GetByEIdAsync(attachment.id!);
            var attachmentEntity = existingAttachmentEntity ?? new AttachmentEntity
            {
                Id = attachment.eid != null ? new Guid(attachment.eid) : Guid.NewGuid(),
                ExternalId = attachment.id ?? Guid.NewGuid().ToString()
            };

            if (attachment.parent?.id != null)
            {
                if (await projectService.ExistsByEIdAsync(attachment.parent.id))
                {
                    attachmentEntity.ProjectId = await projectService.GetIdByEIdAsync(attachment.parent.id);
                }
                else if (await taskService.ExistsByEIdAsync(attachment.parent.id))
                {
                    attachmentEntity.TaskId = await taskService.GetIdByEIdAsync(attachment.parent.id);
                }
                else
                {
                    throw new Exception("PARENT NENÍ");
                }
            }
            else if (existingAttachmentEntity != null)
            {
                attachmentEntity.ProjectId = existingAttachmentEntity.ProjectId;
                attachmentEntity.TaskId = existingAttachmentEntity.TaskId;
            }

            switch (attachment.type)
            {
                case "Create":
                    await attachmentService.AddOrUpdateAsync(attachmentEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = attachmentEntity.Id.ToString(),
                        id = attachmentEntity.ExternalId!
                    });
                    break;
                case "Modify":
                    await attachmentService.AddOrUpdateAsync(attachmentEntity);
                    break;
                case "Delete":
                    await attachmentService.DeleteAsync(new Guid(attachment.eid!));
                    break;
            }
        }

        private async Task HandleUserComponent(syncUserCompany user, IServiceScope scope)
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var oldUser = await userService.GetByEIdAsync(user.id!);
            var userEntity = new UserEntity
            {
                Id = oldUser?.Id ?? Guid.NewGuid(),
                ExternalId = oldUser?.ExternalId ?? user.id,
                Name = oldUser?.Name ?? user.Name,
                Email = oldUser?.Email ?? user.Email
            };

            switch (user.type)
            {
                case "Create":
                    await userService.AddOrUpdateAsync(userEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = userEntity.Id.ToString(),
                        id = userEntity.ExternalId!
                    });
                    break;
                case "Modify":
                    await userService.AddOrUpdateAsync(userEntity);
                    break;
                case "Delete":
                    await userService.DeleteAsync(new Guid(user.eid!));
                    break;
            }
        }

        private async Task HandleCommentComponent(syncComment comment, IServiceScope scope)
        {
            var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();
            var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

            // Retrieve or create a new comment entity
            var commentEntity = await commentService.GetByEIdAsync(comment.id!) ?? new CommentEntity
            {
                Id = comment.eid != null ? new Guid(comment.eid) : Guid.NewGuid(),
                ExternalId = comment.id ?? Guid.NewGuid().ToString(),
                Text = comment.Comment ?? string.Empty,
                Archived = comment.archived ?? false
            };

            // Handle Task association
            if (comment.parent?.id != null)
            {
                if (await taskService.ExistsByEIdAsync(comment.parent.id))
                {
                    commentEntity.TaskId = await taskService.GetIdByEIdAsync(comment.parent.id);
                }
                else
                {
                    throw new Exception("Task does not exist.");
                }
            }

            // Set the UserEmail
            commentEntity.UserEmail = comment.EmailOwner ?? string.Empty;

            // Perform action based on comment type
            switch (comment.type)
            {
                case "Create":
                    await commentService.AddOrUpdateAsync(commentEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = commentEntity.Id.ToString(),
                        id = commentEntity.ExternalId
                    });
                    break;

                case "Modify":
                    await commentService.AddOrUpdateAsync(commentEntity);
                    break;

                case "Delete":
                    await commentService.DeleteAsync(commentEntity.Id);
                    break;
            }
        }

        private async Task HandleTimesheetComponent(syncTimeSheet timeSheet, IServiceScope scope)
        {
            var timeSheetService = scope.ServiceProvider.GetRequiredService<ITimeSheetService>();

            var existingTimeSheetEntity = await timeSheetService.GetByEIdAsync(timeSheet.id!);
            var timeSheetEntity = existingTimeSheetEntity ?? new TimeSheetEntity
            {
                Id = timeSheet.eid != null ? new Guid(timeSheet.eid) : Guid.NewGuid(),
                ExternalId = timeSheet.id ?? Guid.NewGuid().ToString(),
                Description = timeSheet.Description ?? existingTimeSheetEntity?.Description ?? string.Empty,
                Minutes = timeSheet.Minutes ?? existingTimeSheetEntity?.Minutes ?? 0
            };

            switch (timeSheet.type)
            {
                case "Create":
                    await timeSheetService.AddOrUpdateAsync(timeSheetEntity);
                    _externals.Add(new syncExternal
                    {
                        eid = timeSheetEntity.Id.ToString(),
                        id = timeSheetEntity.ExternalId!
                    });
                    break;
                case "Modify":
                    await timeSheetService.AddOrUpdateAsync(timeSheetEntity);
                    break;
                case "Delete":
                    await timeSheetService.DeleteAsync(new Guid(timeSheet.id!));
                    break;
            }
        }

        public async Task<List<ComponentBase>> CreateComponents(List<EntityBase> entities, string type)
        {
            List<ComponentBase> components = new List<ComponentBase>();
            foreach (EntityBase entity in entities)
            {
                switch (entity)
                {
                    case ProjectEntity projectEntity:
                        syncProject project = new syncProject
                        {
                            id = projectEntity.ExternalId ?? null,
                            eid = projectEntity.Id.ToString(),
                            type = type,
                            def = "Project",
                            Name = projectEntity.Name,
                            archived = projectEntity.Archived ?? false,
                            creator = "a06aehv2cg0hoi"
                        };
                        components.Add(project);
                        break;
                    case SectionEntity sectionEntity:
                        syncSection section = new syncSection
                        {
                            id = sectionEntity.ExternalId ?? null,
                            eid = sectionEntity.Id.ToString(),
                            type = type,
                            def = "Section",
                            Name = sectionEntity.Name,
                            creator = "a06aehv2cg0hoi"
                        };
                        components.Add(section);
                        break;
                    case TaskEntity taskEntity:
                        syncTask task = new syncTask
                        {
                            id = taskEntity.ExternalId ?? null,
                            eid = taskEntity.Id.ToString(),
                            type = type,
                            def = "Task",
                            Name = taskEntity.Name,
                            archived = taskEntity.Archived ?? false,
                            creator = "a06aehv2cg0hoi"
                        };
                        components.Add(task);
                        break;
                    case CommentEntity commentEntity:
                        var scope = _serviceProvider.CreateScope();
                        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                        TaskEntity parent = await taskService.GetByIdAsync(commentEntity.TaskId);
                        syncComment comment = new syncComment
                        {
                            id = commentEntity.ExternalId ?? null,
                            eid = commentEntity.Id.ToString(),
                            type = type,
                            def = "Comment",
                            Comment = commentEntity.Text,
                            creator = "a06aehv2cg0hoi",
                            EmailOwner = "david.skrecek@gmail.com",
                            parent = new()
                            {
                                id = parent.ExternalId,
                                field = "Comments"
                            }
                        };
                        components.Add(comment);
                        break;
                    case TimeSheetEntity timeSheetEntity:
                        syncTimeSheet timeSheet = new syncTimeSheet
                        {
                            id = timeSheetEntity.ExternalId ?? null,
                            eid = timeSheetEntity.Id.ToString(),
                            type = type,
                            def = "TimeSheet",
                            Description = timeSheetEntity.Description ?? string.Empty,
                            Minutes = timeSheetEntity.Minutes,
                            creator = "a06aehv2cg0hoi"
                        };
                        components.Add(timeSheet);
                        break;
                    case UserEntity userEntity:
                        syncUserCompany userCompany = new syncUserCompany
                        {
                            id = userEntity.ExternalId ?? null,
                            eid = userEntity.Id.ToString(),
                            def = "UserCompany",
                            Email = userEntity.Email,
                            Name = userEntity.Name,
                            creator = "a06aehv2cg0hoi"
                        };
                        components.Add(userCompany);
                        break;
                }
            }
            return components;
        }

        public async Task HandleResultChanges(List<syncResultChange> results)
        {
            var scope = _serviceProvider.CreateScope();
            var changeTrackingService = scope.ServiceProvider.GetRequiredService<IChangeTrackingService>();
            Guid id;
            foreach (var result in results)
            {
                if (result == null) continue;
                id = new Guid(result.eid);
                switch (result.type)
                {
                    case "Created":
                        await changeTrackingService.AddEId(id, result.addedid);
                        await changeTrackingService.ResetFlags(id);
                        break;
                    case "Modified":
                        await changeTrackingService.ResetFlags(id);
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task InitializeAsync()
        {
            LastTransId = await _apiCommunicator.SyncGetCurrentTranid();
            
            await _apiCommunicator.SyncGetInitialChanges();
        }

        public async Task StoreTransactionId(string tranid)
        {
            try
            { 
                using (var scope = _serviceProvider.CreateScope())
                {
                    var transactionService = scope.ServiceProvider.GetService<ITransactionService>();
                    if (transactionService == null)
                    {
                        Console.WriteLine("TransactionService is not available. Transaction not stored.");
                        return;
                    }

                    TransactionEntity transactionEntity = new()
                    {
                        Id = tranid,
                        DateAdded = DateTime.UtcNow
                    };

                    await transactionService.AddAsyncIfNotExists(transactionEntity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while storing the transaction: {ex.Message}");
            }
        }

        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            var program = serviceProvider.GetRequiredService<Program>();
            await program.InitializeAsync();
        }
    }
}
