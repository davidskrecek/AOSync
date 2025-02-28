using AOSync.DAL.Entities;

namespace AOSync.DAL.Repositories.Interfaces;


public interface IUserService : IRepositoryBase<UserEntity>
{
    public Task<UserEntity?> GetById(Guid id);
    public Task<UserEntity?> GetByEId(string eid);
}