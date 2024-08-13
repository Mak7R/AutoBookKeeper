using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;

public class RolesRepository : Repository<Role, Guid>, IRolesRepository
{
    public RolesRepository(ApplicationDbContext dbContext, ILogger<Repository<Role, Guid>> logger) : base(dbContext, logger)
    {
    }
}