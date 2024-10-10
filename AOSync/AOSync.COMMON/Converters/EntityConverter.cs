using AOSync.COMMON.Model;
using AOSync.DAL.DB;
using Microsoft.Extensions.DependencyInjection;

namespace AOSync.COMMON.Converters;

public class EntityConverter
{
    private readonly IServiceProvider _serviceProvider;

    public EntityConverter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<ComponentBase>> EntitiesToComponents(List<EntityBase> entities, string action)
    {
        var components = new List<ComponentBase>();

        foreach (var entity in entities)
            switch (entity)
            {
                case ProjectEntity projectEntity:
                    var project = new syncProject
                    {
                        id = projectEntity.ExternalId ?? null,
                        eid = projectEntity.Id.ToString(),
                        type = action,
                        def = "Project",
                        Name = projectEntity.Name,
                        archived = projectEntity.Archived ?? false,
                        creator = "a06aehv2cg0hoi"
                    };
                    components.Add(project);
                    break;

                case SectionEntity sectionEntity:
                    var section = new syncSection
                    {
                        id = sectionEntity.ExternalId ?? null,
                        eid = sectionEntity.Id.ToString(),
                        type = action,
                        def = "Section",
                        Name = sectionEntity.Name,
                        creator = "a06aehv2cg0hoi"
                    };
                    components.Add(section);
                    break;

                case TaskEntity taskEntity:
                    var task = new syncTask
                    {
                        id = taskEntity.ExternalId ?? null,
                        eid = taskEntity.Id.ToString(),
                        type = action,
                        def = "Task",
                        Name = taskEntity.Name,
                        archived = taskEntity.Archived ?? false,
                        creator = "a06aehv2cg0hoi"
                    };
                    components.Add(task);
                    break;

                case CommentEntity commentEntity:
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                        var parent = await taskService.GetByIdAsync(commentEntity.TaskId);
                        var comment = new syncComment
                        {
                            id = commentEntity.ExternalId ?? null,
                            eid = commentEntity.Id.ToString(),
                            type = action,
                            def = "Comment",
                            Comment = commentEntity.Text,
                            creator = "a06aehv2cg0hoi",
                            EmailOwner = "david.skrecek@gmail.com",
                            parent = new syncParent
                            {
                                id = parent?.ExternalId,
                                field = "Comments"
                            }
                        };
                        components.Add(comment);
                    }

                    break;

                case TimeSheetEntity timeSheetEntity:
                    var timeSheet = new syncTimeSheet
                    {
                        id = timeSheetEntity.ExternalId ?? null,
                        eid = timeSheetEntity.Id.ToString(),
                        type = action,
                        def = "TimeSheet",
                        Description = timeSheetEntity.Description ?? string.Empty,
                        Minutes = timeSheetEntity.Minutes,
                        creator = "a06aehv2cg0hoi"
                    };
                    components.Add(timeSheet);
                    break;

                case UserEntity userEntity:
                    var userCompany = new syncUserCompany
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

        return components;
    }
}