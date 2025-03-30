using AOSync.APICLIENT;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace AOSync.BL.ProcessingModules;

public class SyncGetInitialChangesResultProcessor : IProcessorBase
{
    private static IServiceProvider _serviceProvider;
    private static SyncSetExternals _externals;
    private static IConfiguration _configuration;
    
    public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task<SyncSetExternals> HandleComponents(ICollection<Components> components)
    {
        using var scope = _serviceProvider.CreateScope();
        SyncSetExternals syncSetExternals = new()
        {
            Company = _configuration.GetValue<string>("Company"),
            Externals = new List<SyncExternal>()
        };

        var prioritizedEntities = new Dictionary<ComponentsDef, List<Components>>
        {
            { ComponentsDef.Workspace, new() },
            { ComponentsDef.Project, new() },
            { ComponentsDef.Team, new() },
            { ComponentsDef.Section, new() },
            { ComponentsDef.Task, new() },
            { ComponentsDef.Comment, new() },
            { ComponentsDef.Attachment, new() },
            { ComponentsDef.TimeSheet, new() },
            { ComponentsDef.UserCompany, new() },
            { ComponentsDef.UserGroup, new() },
            { ComponentsDef.WorkingGroup, new() },
            { ComponentsDef.CommentPredefined, new() },
            // { ComponentsDef.Attribute, new() },
            { ComponentsDef.Phase, new() }
        };

        // Group components by type
        foreach (var component in components)
        {
            if (prioritizedEntities.ContainsKey((ComponentsDef)component.Def!))
            {
                prioritizedEntities[(ComponentsDef)component.Def].Add(component);
            }
            else
            {
                Console.WriteLine($"Warning: Unknown component type {component.Def}");
            }
        }

        // Process each entity type in priority order
        foreach (var entityGroup in prioritizedEntities)
        {
            foreach (var component in entityGroup.Value)
            {
                try
                {
                    var service = GetServiceForComponent(_serviceProvider, component.Def);
                    var parentService = GetParentServiceForComponent(_serviceProvider, component.Def);

                    if (service != null)
                    {
                        var externals = await HandleEntityComponent(component, service, parentService);
                        
                        foreach (var external in externals)
                        {
                            syncSetExternals.Externals.Add(external);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling component {component.Id} of type {component.Def}: {ex.Message}");
                }
            }
        }

        return syncSetExternals;
    }


    private static object GetServiceForComponent(IServiceProvider services, ComponentsDef? def) => def switch
    {
        ComponentsDef.Project => services.GetRequiredService<IProjectRepository>(),
        ComponentsDef.Section => services.GetRequiredService<ISectionRepository>(),
        ComponentsDef.Task => services.GetRequiredService<ITaskRepository>(),
        ComponentsDef.Attachment => services.GetRequiredService<IAttachmentRepository>(),
        ComponentsDef.UserCompany => services.GetRequiredService<IUserRepository>(),
        ComponentsDef.Comment => services.GetRequiredService<ICommentRepository>(),
        ComponentsDef.TimeSheet => services.GetRequiredService<ITimesheetRepository>(),
        ComponentsDef.Workspace => services.GetRequiredService<IWorkspaceRepository>(),
        _ => null!
    };

    private static object? GetParentServiceForComponent(IServiceProvider services, ComponentsDef? def) => def switch
    {
        ComponentsDef.Section or ComponentsDef.Task => services.GetRequiredService<IProjectRepository>(),
        ComponentsDef.Attachment or ComponentsDef.Comment => services.GetRequiredService<ITaskRepository>(),
        _ => null
    };

    private static async Task<List<SyncExternal>> HandleEntityComponent(Components component, object service, object? parentService = null)
    {
        dynamic entityRepository = service;
        var existingEntity = await entityRepository.GetByEIdAsync(component.Id!);
        dynamic entity = existingEntity ?? new 
        { 
            Id = component.Eid != null ? new Guid(component.Eid) : Guid.NewGuid(), 
            ExternalId = component.Id ?? Guid.NewGuid().ToString() 
        };

        var externals = new List<SyncExternal>();
        UpdateEntityProperties(component, entity, existingEntity);

        if (component.Parent?.Id != null && parentService != null)
        {
            dynamic parentSvc = parentService;
            entity.ParentId = await parentSvc.GetIdByEIdAsync(component.Parent.Id);
        }

        await ProcessEntityOperation(component, entityRepository, entity, externals);
        return externals;
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

    private static async Task ProcessEntityOperation(Components component, dynamic entityRepository, dynamic entity, List<SyncExternal> externals)
    {
        switch (component.Type)
        {
            case ComponentsType.Create:
                await entityRepository.AddOrUpdateAsync(entity);
                externals.Add(new SyncExternal { Eid = entity.Id.ToString(), Id = entity.ExternalId });
                break;
            case ComponentsType.Modify:
                await entityRepository.AddOrUpdateAsync(entity);
                break;
            case ComponentsType.Delete:
                await entityRepository.DeleteAsync(entity.Id);
                break;
        }
    }
}
