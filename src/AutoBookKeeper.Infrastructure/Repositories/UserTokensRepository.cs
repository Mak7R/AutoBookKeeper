using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;

public class UserTokensRepository : Repository<UserToken, int>, IUserTokensRepository
{
    public UserTokensRepository(ApplicationDbContext dbContext, ILogger<Repository<UserToken, int>> logger) : base(dbContext, logger)
    {
    }
    
    public async Task RemoveExpiredTokens()
    {
        var now = DateTime.UtcNow;

        var expiredTokens = DbContext.UserTokens.Where(t => t.ExpirationTime < now).ToArray();
        DbContext.UserTokens.RemoveRange(expiredTokens);
        await DbContext.SaveChangesAsync();
    }
}