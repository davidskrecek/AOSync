using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(IDbContextFactory<AOSyncDbContext> factory) : base(factory)
    {
    }
    
    public async Task<UserEntity?> GetById(Guid userId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Users
            .Where(u => u.Id ==userId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<UserEntity?> GetByEId(string userEId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Users
            .Where(u => u.ExternalId ==userEId)
            .FirstOrDefaultAsync();
    }
}