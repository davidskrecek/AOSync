using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class TransactionRepository : RepositoryBase<TransactionEntity>, ITransactionRepository
{
    public TransactionRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }

    public new async Task<TransactionEntity> AddOrUpdateAsync(TransactionEntity transactionEntity)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var transaction = await context.Set<TransactionEntity>().AddAsync(transactionEntity);
        return transaction.Entity;
    }

    public async Task<string?> GetLatestTransactionId()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var latestTransaction = await context.Transactions
            .OrderByDescending(t => t.DateAdded)
            .FirstOrDefaultAsync();

        return latestTransaction?.Id;
    }

    public async Task<TransactionEntity> AddAsyncIfNotExists(TransactionEntity entity)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var existingTransaction = await context.Transactions
            .FirstOrDefaultAsync(t => t.Id == entity.Id);

        if (existingTransaction == null)
        {
            context.Set<TransactionEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        return existingTransaction;
    }
}