using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class TransactionRepository : RepositoryBase<TransactionEntity>, ITransactionService
{
    public TransactionRepository(AOSyncDbContext context) : base(context)
    {
    }

    public async Task<string> GetLatestTransactionId()
    {
        var latestTransaction = await _context.Transactions
            .OrderByDescending(t => t.DateAdded)
            .FirstOrDefaultAsync();

        return latestTransaction?.Id ?? string.Empty;
    }

    public async Task<TransactionEntity> AddAsyncIfNotExists(TransactionEntity entity)
    {
        var existingTransaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == entity.Id);

        if (existingTransaction == null)
        {
            _context.Set<TransactionEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        return existingTransaction;
    }
}