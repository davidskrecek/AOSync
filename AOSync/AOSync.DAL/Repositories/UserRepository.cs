using AOSync.DAL.DatabaseContext;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.Repositories;

public class UserRepository : RepositoryBase<UserEntity>, IUserService
{
    public UserRepository(AOSyncDbContext context) : base(context)
    {
    }
    
    public async Task<UserEntity?> GetById(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id ==userId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<UserEntity?> GetByEId(string userEId)
    {
        return await _context.Users
            .Where(u => u.ExternalId ==userEId)
            .FirstOrDefaultAsync();
    }
}