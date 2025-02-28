using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;

namespace AOSync.DAL.Repositories;

public class UserGroupRepository : RepositoryBase<UserGroupEntity>, IUserGroupRepository
{
    public UserGroupRepository(AOSyncDbContext context) : base(context)
    {
        
    }
}