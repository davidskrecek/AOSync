using AOSync.COMMON.Models;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AOSync.BL.ProcessingModules;

public class SyncGetInitialChangesResultProcessor
{
    private static IServiceProvider _serviceProvider;
    private static SyncSetExternals _externals;
    
    public void Initialize(IServiceProvider serviceProvider, SyncSetExternals externals)
    {
        _serviceProvider = serviceProvider;
        _externals = externals;
    }

    public static async Task HandleComponents(ICollection<Components> components)
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

    private static object GetServiceForComponent(IServiceProvider services, ComponentsDef? def) => def switch
    {
        ComponentsDef.Project => services.GetRequiredService<IProjectRepository>(),
        ComponentsDef.Section => services.GetRequiredService<ISectionService>(),
        ComponentsDef.Task => services.GetRequiredService<ITaskService>(),
        ComponentsDef.Attachment => services.GetRequiredService<IAttachmentRepository>(),
        ComponentsDef.UserCompany => services.GetRequiredService<IUserService>(),
        ComponentsDef.Comment => services.GetRequiredService<ICommentRepository>(),
        ComponentsDef.TimeSheet => services.GetRequiredService<ITimesheetService>(),
        _ => null
    };

    private static object? GetParentServiceForComponent(IServiceProvider services, ComponentsDef? def) => def switch
    {
        ComponentsDef.Section or ComponentsDef.Task => services.GetRequiredService<IProjectRepository>(),
        ComponentsDef.Attachment or ComponentsDef.Comment => services.GetRequiredService<ITaskService>(),
        _ => null
    };

    private static async Task HandleEntityComponent(Components component, object service, object? parentService = null)
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

    private static void UpdateEntityProperties(Components component, dynamic entity, dynamic existingEntity)
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

    private static async Task ProcessEntityOperation(Components component, dynamic entityService, dynamic entity)
    {
        switch (component.Type)
        {
            case ComponentsType.Create:
                await entityService.AddOrUpdateAsync(entity);
                _externals.Externals.Add(new SyncExternal { Eid = entity.Id.ToString(), Id = entity.ExternalId });
                break;
            case ComponentsType.Modify:
                await entityService.AddOrUpdateAsync(entity);
                break;
            case ComponentsType.Delete:
                await entityService.DeleteAsync(entity.Id);
                break;
        }
    }
}
