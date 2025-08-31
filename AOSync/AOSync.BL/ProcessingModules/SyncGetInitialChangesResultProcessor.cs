using AOSync.APICLIENT;
using AOSync.DAL.Repositories.Interfaces;
using AOSync.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace AOSync.BL.ProcessingModules;

public class SyncGetInitialChangesResultProcessor : IProcessorBase
{
    private static IServiceProvider _serviceProvider;
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
            { ComponentsDef.UserCompany, new() },
            { ComponentsDef.Workspace, new() },
            { ComponentsDef.Project, new() },
            { ComponentsDef.Team, new() },
            { ComponentsDef.Section, new() },
            { ComponentsDef.Task, new() },
            { ComponentsDef.Comment, new() },
            { ComponentsDef.Attachment, new() },
            { ComponentsDef.TimeSheet, new() },
            { ComponentsDef.UserGroup, new() },
            { ComponentsDef.WorkingGroup, new() },
            { ComponentsDef.CommentPredefined, new() },
            { ComponentsDef.Phase, new() }
        };

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

        foreach (var entityGroup in prioritizedEntities)
        {
            foreach (var component in entityGroup.Value)
            {
                try
                {
                    var service = GetServiceForComponent(_serviceProvider, component.Def);
                    var entity = CreateEntityForComponent(component);
                    if (service != null && entity != null)
                    {
                        var externals = await HandleEntityComponent(component, service, entity);
                        foreach(var external in externals)
                            syncSetExternals.Externals.Add(external);
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

    private static object CreateEntityForComponent(Components component) => component.Def switch
    {
        ComponentsDef.Project => new ProjectEntity { ExternalId = component.Id },
        ComponentsDef.Section => new SectionEntity { ExternalId = component.Id },
        ComponentsDef.Task => new TaskEntity { ExternalId = component.Id },
        ComponentsDef.Attachment => new AttachmentEntity { ExternalId = component.Id },
        ComponentsDef.UserCompany => new UserEntity { ExternalId = component.Id },
        ComponentsDef.Comment => new CommentEntity { ExternalId = component.Id },
        ComponentsDef.TimeSheet => new TimesheetEntity { ExternalId = component.Id },
        ComponentsDef.Workspace => new WorkspaceEntity { ExternalId = component.Id },
        _ => null!
    };

    private static async Task<List<SyncExternal>> HandleEntityComponent(Components component, object service, object entity)
    {
        dynamic entityRepository = service;
        var existingEntity = await entityRepository.GetByEIdAsync(component.Id!);
        var updatedEntity = existingEntity ?? entity;
        updatedEntity = UpdateEntityProperties(component, updatedEntity);

        var externals = new List<SyncExternal>();
        await ProcessEntityOperation(component, entityRepository, updatedEntity, externals);
        return externals;
    }
    
    // todo foreign keys wrong, need to fix that
    // the prefered approach would be to based of Def use a different constructor

    private static dynamic UpdateEntityProperties(Components component, dynamic entity)
    {
        if (component.AdditionalProperties.TryGetValue("Name", out var name))
        {
            entity.Name = name ?? entity.Name ?? string.Empty;
        }
        if (component.Archived.HasValue)
        {
            entity.Archived = component.Archived.Value;
        }
        return entity;
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
