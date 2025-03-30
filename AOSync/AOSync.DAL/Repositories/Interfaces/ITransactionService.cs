using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface ITransactionRepository : IRepositoryBase<TransactionEntity>
{
    Task<string?> GetLatestTransactionId();
    Task<TransactionEntity> AddAsyncIfNotExists(TransactionEntity entity);
}