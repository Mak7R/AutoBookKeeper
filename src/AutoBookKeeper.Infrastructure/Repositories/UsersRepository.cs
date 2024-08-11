using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;

namespace AutoBookKeeper.Infrastructure.Repositories;


public class UsersRepository : Repository<User, Guid>, IUsersRepository
{
    public UsersRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}