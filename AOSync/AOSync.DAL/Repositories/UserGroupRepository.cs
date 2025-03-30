using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class UserGroupRepository : RepositoryBase<UserGroupEntity>, IUserGroupRepository
{
    public UserGroupRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
        
    }
}