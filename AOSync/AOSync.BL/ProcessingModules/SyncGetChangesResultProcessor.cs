using AOSync.COMMON.Models;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AOSync.BL.ProcessingModules;

public class SyncGetChangesResultProcessor
{
    private static IServiceProvider _serviceProvider;
    private static SyncSetExternals _externals;
    
    public void Initialize(IServiceProvider serviceProvider, SyncSetExternals externals)
    {
        _serviceProvider = serviceProvider;
        _externals = externals;
    }

    public static async Task HandleComponents(ICollection<Changes> components)
    {
        using var scope = _serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        foreach (var component in components)
        {
            try
            {
                var service = GetServiceForComponent(services, component.Def);
                var parentService = GetParentServiceForComponent(services, component.Def);

                if (service != null)
                    await HandleEntityComponent(component, service, parentService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling component {component.Id} of type {component.Def}: {ex.Message}");
            }
        }
    }

    private static object GetServiceForComponent(IServiceProvider services, ChangesDef? def) => def switch
    {
        ChangesDef.Project => services.GetRequiredService<IProjectRepository>(),
        ChangesDef.Section => services.GetRequiredService<ISectionService>(),
        ChangesDef.Task => services.GetRequiredService<ITaskService>(),
        ChangesDef.Attachment => services.GetRequiredService<IAttachmentRepository>(),
        ChangesDef.UserCompany => services.GetRequiredService<IUserService>(),
        ChangesDef.Comment => services.GetRequiredService<ICommentRepository>(),
        ChangesDef.TimeSheet => services.GetRequiredService<ITimesheetService>(),
        _ => null
    };

    private static object? GetParentServiceForComponent(IServiceProvider services, ChangesDef? def) => def switch
    {
        ChangesDef.Section or ChangesDef.Task => services.GetRequiredService<IProjectRepository>(),
        ChangesDef.Attachment or ChangesDef.Comment => services.GetRequiredService<ITaskService>(),
        _ => null
    };

    private static async Task HandleEntityComponent(Changes component, object service, object? parentService = null)
    {
        dynamic entityService = service;
        var existingEntity = await entityService.GetByEIdAsync(component.Id!);
        dynamic entity = existingEntity ?? new { Id = component.Eid != null ? new Guid(component.Eid) : Guid.NewGuid(), ExternalId = component.Id ?? Guid.NewGuid().ToString() };

        UpdateEntityProperties(component, entity, existingEntity);

        if (component.Parent?.Id != null && parentService != null)
        {
            dynamic parentSvc = parentService;
            entity.ParentId = await parentSvc.GetIdByEIdAsync(component.Parent.Id);
        }

        await ProcessEntityOperation(component, entityService, entity);
    }

    private static void UpdateEntityProperties(Changes component, dynamic entity, dynamic existingEntity)
    {
        if (component.AdditionalProperties.TryGetValue("Name", out var name))
        {
            entity.Name = name?.ToString() ?? existingEntity?.Name ?? string.Empty;
        }

        if (component.Archived.HasValue)
        {
            entity.Archived = component.Archived.Value;
        }
        else if (existingEntity != null)
        {
            entity.Archived = existingEntity.Archived;
        }
    }

    private static async Task ProcessEntityOperation(Changes component, dynamic entityService, dynamic entity)
    {
        switch (component.Type)
        {
            case ChangesType.Create:
                await entityService.AddOrUpdateAsync(entity);
                _externals.Externals.Add(new SyncExternal { Eid = entity.Id.ToString(), Id = entity.ExternalId });
                break;
            case ChangesType.Modify:
                await entityService.AddOrUpdateAsync(entity);
                break;
            case ChangesType.Delete:
                await entityService.DeleteAsync(entity.Id);
                break;
        }
    }
}
