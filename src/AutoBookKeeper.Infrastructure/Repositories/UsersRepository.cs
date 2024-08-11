using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;


public class UsersRepository : Repository<User, Guid>, IUsersRepository
{
    public UsersRepository(ApplicationDbContext dbContext, ILogger<UsersRepository> logger) : base(dbContext, logger)
    {
    }
}