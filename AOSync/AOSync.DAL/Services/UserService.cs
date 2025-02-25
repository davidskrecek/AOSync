using Microsoft.EntityFrameworkCore;

namespace AOSync.DAL.DB;

public class UserService : DataService<UserEntity>, IUserService
{
    public UserService(AOSyncDbContext context) : base(context)
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