using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using AOSync.BL.Services;
using AOSync.COMMON.Converters;
using AOSync.COMMON.ApiClient;
using AOSync.COMMON.Models;
using AOSync.DAL.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IServiceScope = Microsoft.Extensions.DependencyInjection.IServiceScope;

namespace AOSync.BL;

public class BusinessManager
{
    private readonly IConfiguration _configuration;
    private readonly DataReloadService _dataReloadService;
    private SyncSetExternals _externals { get; set; }
    private readonly IServiceProvider _serviceProvider;
    private readonly ISynchronizationApiClient _iSynchronizationApiClient;

    private readonly ITransactionService _iTransactionService;
    private SyncGetChanges _syncGetChanges { get; set; }
    private SyncGetInitialChanges _syncGetInitialChanges { get; set; }
    private bool _dataChanged;

    public BusinessManager(IConfiguration configuration, IServiceProvider serviceProvider, DataReloadService dataReloadService)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _dataReloadService = dataReloadService;
        
        _iSynchronizationApiClient = _serviceProvider.GetRequiredService<ISynchronizationApiClient>();
        _iTransactionService = _serviceProvider.GetRequiredService<ITransactionService>();

        _dataChanged = false;

        _externals = new ();
        _syncGetChanges = new();
        _syncGetInitialChanges = new();
    }

    public string? LastTransId { get; set; }

    public async Task HandleComponents(ICollection<Components> components)
    {
        foreach (var component in components)
        {
            using var scope = _serviceProvider.CreateScope();

            switch (component.Def)
            {
                case ComponentsDef.Project:
                    await HandleProjectComponent(component, scope);
                    _dataChanged = true;
                    break;
                case ComponentsDef.Section:
                    await HandleSectionComponent(component, scope);
                    _dataChanged = true;
                    break;
                case ComponentsDef.Task:
                    await HandleTaskComponent(component, scope);
                    _dataChanged = true;
                    break;
                case ComponentsDef.Attachment:
                    await HandleAttachmentComponent(component, scope);
                    _dataChanged = true;
                    break;
                case ComponentsDef.UserCompany:
                    await HandleUserComponent(component, scope);
                    _dataChanged = true;
                    break;
                case ComponentsDef.Comment:
                    await HandleCommentComponent(component, scope);
                    _dataChanged = true;
                    break;
                case ComponentsDef.TimeSheet:
                    await HandleTimesheetComponent(component, scope);
                    _dataChanged = true;
                    break;
                default:
                    continue;
            }
        }

        if (_dataChanged)
        {
            await _iSynchronizationApiClient.SyncSetExternalsAsync(_externals);
            _externals.Externals.Clear();
            _dataReloadService.NotifyDataChanged();
            _dataChanged = false;
        }
    }

    private async Task HandleProjectComponent(Components project, IServiceScope scope)
    {
        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
        var existingProjectEntity = await projectService.GetByEIdAsync(project.Id!);

        var projectEntity = existingProjectEntity ?? new ProjectEntity
        {
            Id = project.Eid != null ? new Guid(project.Eid) : Guid.NewGuid(),
            ExternalId = project.Id ?? Guid.NewGuid().ToString()
        };

        projectEntity.Name = project.AdditionalProperties["Name"].ToString() ?? existingProjectEntity?.Name ?? string.Empty;
        projectEntity.Archived = project.Archived ?? existingProjectEntity?.Archived ?? false;

        switch (project.Type)
        {
            case ComponentsType.Create:
                await projectService.AddOrUpdateAsync(projectEntity);
                _externals.Externals.Add(new SyncExternal
                {
                    Eid = projectEntity.Id.ToString(),
                    Id = projectEntity.ExternalId!
                });
                break;
            case ComponentsType.Modify:
                await projectService.AddOrUpdateAsync(projectEntity);
                break;
            case ComponentsType.Delete:
                await projectService.DeleteAsync(projectEntity.Id);
                break;
        }
    }


    private async Task HandleSectionComponent(Components section, IServiceScope scope)
    {
        var sectionService = scope.ServiceProvider.GetRequiredService<ISectionService>();
        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();

        var existingSectionEntity = await sectionService.GetByEIdAsync(section.Id!);
        var sectionEntity = existingSectionEntity ?? new SectionEntity
        {
            Id = section.Eid != null ? new Guid(section.Eid) : Guid.NewGuid(),
            ExternalId = section.Id ?? Guid.NewGuid().ToString()
        };

        sectionEntity.Name = section.AdditionalProperties["Name"].ToString() ?? existingSectionEntity?.Name ?? string.Empty;

        if (section.Parent?.Id != null)
        {
            var parentProjectId = await projectService.GetIdByEIdAsync(section.Parent.Id);
            sectionEntity.ProjectId = parentProjectId;
        }
        else if (existingSectionEntity != null)
        {
            sectionEntity.ProjectId = existingSectionEntity.ProjectId;
        }

        switch (section.Type)
        {
            case ComponentsType.Create:
                await sectionService.AddOrUpdateAsync(sectionEntity);
                _externals.Externals.Add(new SyncExternal
                {
                    Eid = sectionEntity.Id.ToString(),
                    Id = sectionEntity.ExternalId!
                });
                break;
            case ComponentsType.Modify:
                await sectionService.AddOrUpdateAsync(sectionEntity);
                break;
            case ComponentsType.Delete:
                await sectionService.DeleteAsync(sectionEntity.Id);
                break;
        }
    }


    private async Task HandleTaskComponent(Components task, IServiceScope scope)
    {
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
        var sectionService = scope.ServiceProvider.GetRequiredService<ISectionService>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var existingTaskEntity = await taskService.GetByEIdAsync(task.Id!);

        var taskEntity = existingTaskEntity ?? new TaskEntity
        {
            Id = task.Eid != null ? new Guid(task.Eid) : Guid.NewGuid(),
            ExternalId = task.Id ?? Guid.NewGuid().ToString()
        };

        taskEntity.Name = task.AdditionalProperties["Name"].ToString() ?? existingTaskEntity?.Name ?? string.Empty;
        taskEntity.Archived = task.Archived ?? existingTaskEntity?.Archived ?? false;

        // Set Section relationship
        if (task.Parent?.Id != null)
        {
            taskEntity.SectionId = await sectionService.GetIdByEIdAsync(task.Parent.Id);
        }
        else if (existingTaskEntity != null)
        {
            taskEntity.SectionId = existingTaskEntity.SectionId;
        }

        // Set Owner
        if (task.AdditionalProperties["Owner"] != null)
        {
            var owner = await userService.GetByEIdAsync(task.AdditionalProperties["Owner"].ToString()!);
            if (owner != null)
            {
                taskEntity.Owner = owner;
            }
        }

        switch (task.Type)
        {
            case ComponentsType.Create:
                await taskService.AddOrUpdateAsync(taskEntity);
                AddToExternals(taskEntity.Id.ToString(), taskEntity.ExternalId!);
                break;
            case ComponentsType.Modify:
                await taskService.AddOrUpdateAsync(taskEntity);
                break;
            case ComponentsType.Delete:
                await taskService.DeleteAsync(taskEntity.Id);
                break;
        }
    }

    private async Task HandleAttachmentComponent(Components attachment, IServiceScope scope)
    {
        var attachmentService = scope.ServiceProvider.GetRequiredService<IAttachmentService>();
        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var existingAttachmentEntity = await attachmentService.GetByEIdAsync(attachment.Id!);
        var attachmentEntity = existingAttachmentEntity ?? new AttachmentEntity
        {
            Id = attachment.Eid != null ? new Guid(attachment.Eid) : Guid.NewGuid(),
            ExternalId = attachment.Id ?? Guid.NewGuid().ToString()
        };

        if (attachment.Parent?.Id != null)
        {
            if (await projectService.ExistsByEIdAsync(attachment.Parent.Id))
                attachmentEntity.ProjectId = await projectService.GetIdByEIdAsync(attachment.Parent.Id);
            else if (await taskService.ExistsByEIdAsync(attachment.Parent.Id))
                attachmentEntity.TaskId = await taskService.GetIdByEIdAsync(attachment.Parent.Id);
            else
                throw new Exception("Parent entity not found.");
        }
        else if (existingAttachmentEntity != null)
        {
            attachmentEntity.ProjectId = existingAttachmentEntity.ProjectId;
            attachmentEntity.TaskId = existingAttachmentEntity.TaskId;
        }

        switch (attachment.Type)
        {
            case ComponentsType.Create:
                await attachmentService.AddOrUpdateAsync(attachmentEntity);
                _externals.Externals.Add(new SyncExternal
                {
                    Eid = attachmentEntity.Id.ToString(),
                    Id = attachmentEntity.ExternalId!
                });
                break;
            case ComponentsType.Modify:
                await attachmentService.AddOrUpdateAsync(attachmentEntity);
                break;
            case ComponentsType.Delete:
                await attachmentService.DeleteAsync(attachmentEntity.Id);
                break;
        }
    }


    private async Task HandleUserComponent(Components user, IServiceScope scope)
    {
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var oldUser = await userService.GetByEIdAsync(user.Id!);
        var userEntity = new UserEntity
        {
            Id = oldUser?.Id ?? Guid.NewGuid(),
            ExternalId = user.Id ?? oldUser?.ExternalId!,
            Name = user.AdditionalProperties["Name"].ToString() ?? oldUser?.Name!,
            Email = user.AdditionalProperties["Email"].ToString() ??  oldUser?.Email!
        };

        switch (user.Type)
        {
            case ComponentsType.Create:
                await userService.AddOrUpdateAsync(userEntity);
                _externals.Externals.Add(new SyncExternal
                {
                    Eid = userEntity.Id.ToString(),
                    Id = userEntity.ExternalId!
                });
                break;
            case ComponentsType.Modify:
                await userService.AddOrUpdateAsync(userEntity);
                break;
            case ComponentsType.Delete:
                await userService.DeleteAsync(new Guid(user.Eid!));
                break;
        }
    }

    private async Task HandleCommentComponent(Components comment, IServiceScope scope)
    {
        var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var existingCommentEntity = await commentService.GetByEIdAsync(comment.Id!);
        var commentEntity = existingCommentEntity ?? new CommentEntity
        {
            Id = comment.Eid != null ? new Guid(comment.Eid) : Guid.NewGuid(),
            ExternalId = comment.Id ?? Guid.NewGuid().ToString()
        };

        commentEntity.Text = comment.AdditionalProperties["Comment"].ToString() ?? existingCommentEntity?.Text ?? string.Empty;

        if (comment.Parent?.Id != null)
        {
            var taskId = await taskService.GetIdByEIdAsync(comment.Parent.Id);
            commentEntity.TaskId = taskId;
        }

        switch (comment.Type)
        {
            case ComponentsType.Create:
                await commentService.AddOrUpdateAsync(commentEntity);
                _externals.Externals.Add(new SyncExternal
                {
                    Eid = commentEntity.Id.ToString(),
                    Id = commentEntity.ExternalId!
                });
                break;
            case ComponentsType.Modify:
                await commentService.AddOrUpdateAsync(commentEntity);
                break;
            case ComponentsType.Delete:
                await commentService.DeleteAsync(commentEntity.Id);
                break;
        }
    }


    private async Task HandleTimesheetComponent(Components timeSheet, IServiceScope scope)
    {
        var timeSheetService = scope.ServiceProvider.GetRequiredService<ITimesheetService>();

        var existingTimesheetEntity = await timeSheetService.GetByEIdAsync(timeSheet.Id!);
        double minutes = Convert.ToDouble(timeSheet.AdditionalProperties["Minutes"].ToString());
        var timeSheetEntity = existingTimesheetEntity ?? new TimesheetEntity
        {
            Id = timeSheet.Eid != null ? new Guid(timeSheet.Eid) : Guid.NewGuid(),
            ExternalId = timeSheet.Id ?? Guid.NewGuid().ToString(),
            Description = timeSheet.AdditionalProperties["Description"].ToString() ?? existingTimesheetEntity?.Description ?? string.Empty,
            Minutes = (double)(minutes == 0.0 ? existingTimesheetEntity?.Minutes! : minutes)
        };

        switch (timeSheet.Type)
        {
            case ComponentsType.Create:
                await timeSheetService.AddOrUpdateAsync(timeSheetEntity);
                _externals.Externals.Add(new SyncExternal
                {
                    Eid = timeSheetEntity.Id.ToString(),
                    Id = timeSheetEntity.ExternalId!
                });
                break;
            case ComponentsType.Modify:
                await timeSheetService.AddOrUpdateAsync(timeSheetEntity);
                break;
            case ComponentsType.Delete:
                await timeSheetService.DeleteAsync(new Guid(timeSheet.Id!));
                break;
        }
    }

    // public async Task<List<ComponentBase>> CreateComponents(List<EntityBase> entities, string type)
    // {
    //     var components = new List<ComponentBase>();
    //     foreach (var entity in entities)
    //         switch (entity)
    //         {
    //             case ProjectEntity projectEntity:
    //                 var project = new SyncProject
    //                 {
    //                     id = projectEntity.ExternalId ?? null,
    //                     eid = projectEntity.Id.ToString(),
    //                     type = type,
    //                     def = "Project",
    //                     Name = projectEntity.Name,
    //                     archived = projectEntity.Archived,
    //                     creator = "a06aehv2cg0hoi"
    //                 };
    //                 components.Add(project);
    //                 break;
    //             case SectionEntity sectionEntity:
    //                 var section = new SyncSection
    //                 {
    //                     id = sectionEntity.ExternalId ?? null,
    //                     eid = sectionEntity.Id.ToString(),
    //                     type = type,
    //                     def = "Section",
    //                     Name = sectionEntity.Name,
    //                     creator = "a06aehv2cg0hoi"
    //                 };
    //                 components.Add(section);
    //                 break;
    //             case TaskEntity taskEntity:
    //                 var task = new SyncTask
    //                 {
    //                     id = taskEntity.ExternalId ?? null,
    //                     eid = taskEntity.Id.ToString(),
    //                     type = type,
    //                     def = "Task",
    //                     Name = taskEntity.Name,
    //                     archived = taskEntity.Archived,
    //                     creator = "a06aehv2cg0hoi"
    //                 };
    //                 components.Add(task);
    //                 break;
    //             case CommentEntity commentEntity:
    //                 var scope = _serviceProvider.CreateScope();
    //                 var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
    //                 var parent = await taskService.GetByIdAsync(commentEntity.TaskId);
    //                 var comment = new SyncComment
    //                 {
    //                     id = commentEntity.ExternalId ?? null,
    //                     eid = commentEntity.Id.ToString(),
    //                     type = type,
    //                     def = "Comment",
    //                     Comment = commentEntity.Text,
    //                     creator = "a06aehv2cg0hoi",
    //                     EmailOwner = "david.skrecek@gmail.com",
    //                     parent = new SyncParent
    //                     {
    //                         id = parent.ExternalId,
    //                         field = "Comments"
    //                     }
    //                 };
    //                 components.Add(comment);
    //                 break;
    //             case TimesheetEntity timeSheetEntity:
    //                 var timeSheet = new SyncTimeSheet
    //                 {
    //                     id = timeSheetEntity.ExternalId ?? null,
    //                     eid = timeSheetEntity.Id.ToString(),
    //                     type = type,
    //                     def = "TimeSheet",
    //                     Description = timeSheetEntity.Description ?? string.Empty,
    //                     Minutes = timeSheetEntity.Minutes,
    //                     creator = "a06aehv2cg0hoi"
    //                 };
    //                 components.Add(timeSheet);
    //                 break;
    //             case UserEntity userEntity:
    //                 var userCompany = new SyncUserCompany
    //                 {
    //                     id = userEntity.ExternalId ?? null,
    //                     eid = userEntity.Id.ToString(),
    //                     def = "UserCompany",
    //                     Email = userEntity.Email,
    //                     Name = userEntity.Name,
    //                     creator = "a06aehv2cg0hoi"
    //                 };
    //                 components.Add(userCompany);
    //                 break;
    //         }
    //
    //     return components;
    // }

    // public async Task HandleResultChanges(List<SyncResultChange> results)
    // {
    //     var scope = _serviceProvider.CreateScope();
    //     var changeTrackingService = scope.ServiceProvider.GetRequiredService<IChangeTrackingService>();
    //     Guid id;
    //     foreach (var result in results)
    //     {
    //         id = new Guid(result.eid);
    //         switch (result.type)
    //         {
    //             case "Created":
    //                 await changeTrackingService.AddEId(id, result.addedid);
    //                 await changeTrackingService.ResetFlags(id);
    //                 break;
    //             case "Modified":
    //                 await changeTrackingService.ResetFlags(id);
    //                 break;
    //         }
    //     }
    // }

    public async Task SetChanges()
    {
        #region CollectChanges
        var scope = _serviceProvider.CreateScope();
        var changeTrackingService = scope.ServiceProvider.GetRequiredService<IChangeTrackingService>();
        var changedEntities = await changeTrackingService!.GetChangedRecordsAsync();
        var createdEntities = await changeTrackingService!.GetCreatedRecordsAsync();
        var converter = new EntityToApiClassConverter(_serviceProvider, _configuration);
        var changes = await converter.CreatedEntitiesToApiClassAsync(createdEntities);

        SyncSetChanges syncSetChanges = new()
        {
            Company = _configuration.GetValue<string>("CompanyID"),
            Usercompany = _configuration.GetValue<string>("UserCompanyID"),
            Changes = changes
        };

        #endregion
        
        #region SentChanges
        var results = await _iSynchronizationApiClient.SyncSetChangesAsync(syncSetChanges);
        #endregion
        
        // TODO - bellow 
        
        // #region IterateResults
        // foreach (var result in results)
        // {
        //     switch (result.type)
        //     {
        //         case "Created":
        //             Guid id = new Guid(result.eid);
        //             await changeTrackingService.AddEId(id, result.addedid);
        //             await changeTrackingService.ResetFlags(id);
        //             break;
        //         case "Modified":
        //             await changeTrackingService.ResetFlags(new Guid(result.eid));
        //             break;
        //         case "Deleted":
        //             await changeTrackingService.ResetFlags(new Guid(result.eid));
        //             break;
        //         default:
        //             break;
        //     }
        // }
        // #endregion
    }

    public async Task GetChanges()
    {
        // SyncGetChanges result = null;
        // List<Changes> changes = await _iSynchronizationApiClient.SyncGetChangesAsync();
        // await HandleComponents(changes);
    }

    private async Task<string> GetLastTransactionId()
    {
        return await _iTransactionService.GetLatestTransactionId();
    }

    public async Task GetInitialChanges()
    {
        SyncGetCurrentTranid tranidRequest = new SyncGetCurrentTranid();
        if (_configuration.GetValue<bool>("SystemInitialization"))
        {
            LastTransId = await GetLastTransactionId();

            if (string.IsNullOrEmpty(LastTransId))
            {
                tranidRequest.Company = _configuration.GetValue<string>("CompanyID");
                SyncGetCurrentTranidResult tranidResult =  await _iSynchronizationApiClient.SyncGetCurrentTranidAsync(tranidRequest);
                LastTransId = tranidResult.Tranid;
            }
        }

        SyncGetInitialChanges initialChangesRequest = new SyncGetInitialChanges()
        {
            Company = _configuration.GetValue<string>("CompanyID"),
            Maxtranid = LastTransId,
            Withexternalid = _configuration.GetValue<bool>("Withexternalid"),
            
        };

        SyncGetInitialChangesResult initialChangesResult = await _iSynchronizationApiClient.SyncGetInitialChangesAsync(initialChangesRequest);
        Console.WriteLine(initialChangesRequest.ToJson());
        await HandleComponents(initialChangesResult.Components);
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
    
    private void AddToExternals(string eid, string externalId)
    {
        _externals.Externals.Add(new SyncExternal
        {
            Eid = eid,
            Id = externalId
        });
    }
}