namespace AOSync.DAL.DB;

public interface IDataService<T>
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByEIdAsync(string eid);
    Task<Guid> GetIdByEIdAsync(string eid);
    Task<T> AddAsync(T entity);
    Task<T> AddOrUpdateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> AddEId(Guid id, string eid);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByEIdAsync(string eid);
}

public interface IBaseService : IDataService<EntityBase> { }

public interface IProjectService : IDataService<ProjectEntity>
{
}

public interface ISectionService : IDataService<SectionEntity>
{
    Task<ICollection<SectionEntity>> GetSectionsByProjectId(Guid projectId);
}

public interface ITaskService : IDataService<TaskEntity>
{
    Task<ICollection<TaskEntity>> GetTasksBySectionId(Guid sectionId);
}

public interface IUserService : IDataService<UserEntity>
{
    public Task<UserEntity?> GetById(Guid id);
    public Task<UserEntity?> GetByEId(string eid);
}

public interface ICommentService : IDataService<CommentEntity>
{
    Task<ICollection<CommentEntity>> GetCommentsByTaskId(Guid taskId);
}

public interface IAttachmentService : IDataService<AttachmentEntity>
{
    Task<ICollection<AttachmentEntity>> GetAttachmentsByTaskId(Guid taskId);
}

public interface ITransactionService : IDataService<TransactionEntity>
{
    Task<string> GetLatestTransactionId();
    Task<TransactionEntity> AddAsyncIfNotExists(TransactionEntity entity);
}

public interface ITimesheetService : IDataService<TimesheetEntity>
{
}

public interface IChangeTrackingService : IDataService<EntityBase>
{
    Task<ICollection<EntityBase>> GetChangedRecordsAsync();
    Task<ICollection<EntityBase>> GetCreatedRecordsAsync();
    Task<ICollection<EntityBase>> GetDeletedRecordsAsync();
    Task ResetFlags(Guid id);
}