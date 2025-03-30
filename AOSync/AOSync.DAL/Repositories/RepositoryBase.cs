using System.Linq.Expressions;
using System.Reflection;
using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly IDbContextFactory<AOSyncDbContext> _contextFactory;

    public RepositoryBase(IDbContextFactory<AOSyncDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<ICollection<T>> GetAllAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetByEIdAsync(string eid)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "ExternalId") == eid);
    }

    public async Task<Guid> GetIdByEIdAsync(string eid)
    {
        using var context = _contextFactory.CreateDbContext();
        var entityId = await context.Set<T>()
            .Where(e => EF.Property<string>(e, "ExternalId") == eid)
            .Select(e => EF.Property<Guid>(e, "Id"))
            .FirstOrDefaultAsync();

        return entityId;
    }


    public async Task<T> AddAsync(T entity)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> AddOrUpdateAsync(T entity)
    {
        using var context = _contextFactory.CreateDbContext();
        // Extract the Id and ExternalId from the entity
        var id = (Guid)entity.GetType().GetProperty("Id")!.GetValue(entity)!;
        var externalId = (string)entity.GetType().GetProperty("ExternalId")!.GetValue(entity)!;

        // Check for an existing entity using the Id or ExternalId
        var existingEntity = await context.Set<T>().FirstOrDefaultAsync(e =>
            EF.Property<Guid>(e, "Id") == id || EF.Property<string>(e, "ExternalId") == externalId);

        if (existingEntity == null)
        {
            context.Set<T>().Add(entity);
        }
        else
        {
            var entityEntry = context.Entry(existingEntity);
            var entityType = context.Model.FindEntityType(typeof(T));
            var keyProperties = entityType!.FindPrimaryKey()!.Properties;

            foreach (var property in entityType.GetProperties())
                if (!keyProperties.Contains(property))
                {
                    var newValue = entity.GetType().GetProperty(property.Name)?.GetValue(entity);
                    if (newValue != null) entityEntry.Property(property.Name).CurrentValue = newValue;
                }
        }

        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> AddEId(Guid id, string eid)
    {
        using var context = _contextFactory.CreateDbContext();
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null) return false;

        var entityType = context.Model.FindEntityType(typeof(T));
        if (entityType == null)
            throw new InvalidOperationException($"The entity type '{typeof(T).Name}' is not mapped in the DbContext.");

        var eidProperty = entityType.FindProperty("ExternalId");
        if (eidProperty == null)
            throw new InvalidOperationException(
                $"The property 'ExternalId' does not exist on entity type '{typeof(T).Name}'.");

        context.Entry(entity).Property("ExternalId").CurrentValue = eid;
        await context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null) return false;

        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Set<T>().FindAsync(id) != null;
    }

    public async Task<bool> ExistsByEIdAsync(string eid)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Set<T>().AnyAsync(e => EF.Property<string>(e, "ExternalId") == eid);
    }
    
    public async Task AddEIdByIdAsync(Guid id, string eid)
    {
        using var context = _contextFactory.CreateDbContext();
        // Get all entity types in the DbContext
        var entityTypes = context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            // Get the CLR type of the entity (class)
            var clrType = entityType.ClrType;

            // Check if the entity has both "Id" and "ExternalId" properties
            var idProperty = entityType.FindProperty("Id");
            var externalIdProperty = entityType.FindProperty("ExternalId");

            // If both "Id" and "ExternalId" exist and the "Id" is of type Guid, proceed
            if (idProperty != null && externalIdProperty != null && idProperty.ClrType == typeof(Guid))
            {
                // Use reflection to get the DbSet for the current entity type
                var method = typeof(AOSyncDbContext).GetMethod("Set", BindingFlags.Public | BindingFlags.Instance);
                var genericMethod = method!.MakeGenericMethod(clrType);
                var dbSet = genericMethod.Invoke(context, null); // Equivalent to context.Set(clrType)

                // Create an expression to query by Id
                var parameter = Expression.Parameter(clrType, "e");
                var idExpression = Expression.Equal(
                    Expression.Property(parameter, "Id"),
                    Expression.Constant(id)
                );

                var lambda = Expression.Lambda(idExpression, parameter);

                // Use reflection to call IQueryable.Where dynamically
                var whereMethod = typeof(Queryable).GetMethods()
                    .Where(m => m.Name == "Where" && m.GetParameters().Length == 2)
                    .Single()
                    .MakeGenericMethod(clrType);

                var query = whereMethod.Invoke(null, new object[] { dbSet, lambda });

                // Use reflection to call FirstOrDefaultAsync dynamically
                var firstOrDefaultMethod = typeof(EntityFrameworkQueryableExtensions)
                    .GetMethod("FirstOrDefaultAsync", new[] { typeof(IQueryable<>) })
                    .MakeGenericMethod(clrType);

                var task = (Task)firstOrDefaultMethod.Invoke(null, new object[] { query });
                await task.ConfigureAwait(false);

                var entity = task.GetType().GetProperty("Result")!.GetValue(task);

                if (entity != null)
                {
                    // Update the ExternalId
                    context.Entry(entity).Property("ExternalId").CurrentValue = eid;

                    // Save changes
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}