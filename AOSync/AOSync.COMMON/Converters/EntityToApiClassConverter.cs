using AOSync.COMMON.Models;
using AOSync.DAL.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AOSync.COMMON.Converters;

public class EntityToApiClassConverter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public EntityToApiClassConverter(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task<ICollection<changes>> CreatedEntitiesToApiClassAsync(ICollection<EntityBase> entities)
    {
        List<changes> components = new();

        foreach (var entity in entities)
        {
            switch (entity)
            {
                case AttachmentEntity attachmentEntity:
                    components.Add(new changes
                    {
                        Id = attachmentEntity.ExternalId,
                        Eid = attachmentEntity.Id.ToString(),
                        Type = changesType.Create,
                        Def = changesDef.Attachment,
                        Creator = _configuration.GetValue<string>("UserCompanyID"),
                        Parent = ResolveParent(
                            attachmentEntity.ProjectId,
                            attachmentEntity.TaskId,
                            attachmentEntity.CommentId)
                    });
                    break;

                case CommentEntity commentEntity:
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                        var parentTask = await taskService.GetByIdAsync((Guid)commentEntity.TaskId);

                        components.Add(new changes
                        {
                            Id = commentEntity.ExternalId,
                            Eid = commentEntity.Id.ToString(),
                            Type = changesType.Create,
                            Def = changesDef.Comment,
                            Creator = _configuration.GetValue<string>("UserCompanyID"),
                            Parent = new SyncParent
                            {
                                Id = parentTask?.ExternalId,
                                Field = "Comments",
                                Def = SyncParentDef.Task,
                                Eid = parentTask?.ExternalId
                            },
                            AdditionalProperties = new Dictionary<string, object>
                            {
                                { "Comment", commentEntity.Text }
                            }
                        });
                    }
                    break;

                case TaskEntity taskEntity:
                    components.Add(new changes
                    {
                        Id = taskEntity.ExternalId,
                        Eid = taskEntity.Id.ToString(),
                        Type = changesType.Create,
                        Def = changesDef.Task,
                        Creator = _configuration.GetValue<string>("UserCompanyID"),
                        Parent = taskEntity.SectionId != Guid.Empty
                            ? new SyncParent
                            {
                                Id = taskEntity.SectionId.ToString(),
                                Field = "Tasks",
                                Def = SyncParentDef.Section
                            }
                            : null
                    });
                    break;

                default:
                    throw new InvalidOperationException($"Unknown entity type: {entity.GetType().Name}");
            }
        }

        return components;
    }

    public async Task<List<changes>> EntitiesToComponents(ICollection<EntityBase> entities, string action)
    {
        var components = new List<changes>();

        foreach (var entity in entities)
        {
            switch (entity)
            {
                case AttachmentEntity attachmentEntity:
                    components.Add(new changes
                    {
                        Id = attachmentEntity.ExternalId,
                        Eid = attachmentEntity.Id.ToString(),
                        Type = ResolveChangeType(action),
                        Def = changesDef.Attachment,
                        Creator = _configuration.GetValue<string>("UserCompanyID"),
                        Parent = ResolveParent(
                            attachmentEntity.ProjectId,
                            attachmentEntity.TaskId,
                            attachmentEntity.CommentId)
                    });
                    break;

                case CommentEntity commentEntity:
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                        var parentTask = await taskService.GetByIdAsync((Guid)commentEntity.TaskId);

                        components.Add(new changes
                        {
                            Id = commentEntity.ExternalId,
                            Eid = commentEntity.Id.ToString(),
                            Type = ResolveChangeType(action),
                            Def = changesDef.Comment,
                            Creator = _configuration.GetValue<string>("UserCompanyID"),
                            Parent = new SyncParent
                            {
                                Id = parentTask?.ExternalId,
                                Field = "Comments",
                                Def = SyncParentDef.Task,
                                Eid = parentTask?.ExternalId
                            },
                            AdditionalProperties = new Dictionary<string, object>
                            {
                                { "Comment", commentEntity.Text }
                            }
                        });
                    }
                    break;

                case TaskEntity taskEntity:
                    components.Add(new changes
                    {
                        Id = taskEntity.ExternalId,
                        Eid = taskEntity.Id.ToString(),
                        Type = ResolveChangeType(action),
                        Def = changesDef.Task,
                        Creator = _configuration.GetValue<string>("UserCompanyID"),
                        Parent = taskEntity.SectionId != Guid.Empty
                            ? new SyncParent
                            {
                                Id = taskEntity.SectionId.ToString(),
                                Field = "Tasks",
                                Def = SyncParentDef.Section
                            }
                            : null
                    });
                    break;
                
                case ProjectEntity projectEntity:
                    components.Add(new changes
                    {
                        Id = projectEntity.ExternalId,
                        Eid = projectEntity.Id.ToString(),
                        Type = ResolveChangeType(action),
                        Def = changesDef.Project,
                        Creator = _configuration.GetValue<string>("UserCompanyID"),
                        Parent =
                        {
                            
                        }
                    });
                    break;

                default:
                    throw new InvalidOperationException($"Unknown entity type: {entity.GetType().Name}");
            }
        }

        return components;
    }

    private changesType ResolveChangeType(string action)
    {
        switch (action)
        {
            case "Create":
                return changesType.Create;
            case "Modify":
                return changesType.Modify;
            default:
                return changesType.Delete;
        }
    }

    private SyncParent? ResolveParent(Guid? projectId, Guid? taskId, Guid? commentId)
    {
        if (projectId.HasValue)
        {
            return new SyncParent
            {
                Id = projectId.ToString(),
                Field = "Attachments",
                Def = SyncParentDef.Project
            };
        }

        if (taskId.HasValue)
        {
            return new SyncParent
            {
                Id = taskId.ToString(),
                Field = "Attachments",
                Def = SyncParentDef.Task
            };
        }

        if (commentId.HasValue)
        {
            return new SyncParent
            {
                Id = commentId.ToString(),
                Field = "Comments",
                Def = SyncParentDef.Comment
            };
        }

        return null;
    }
}
