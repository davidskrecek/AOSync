using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface ITransactionService : IRepositoryBase<TransactionEntity>
{
    Task<string> GetLatestTransactionId();
    Task<TransactionEntity> AddAsyncIfNotExists(TransactionEntity entity);
}