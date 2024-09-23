using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;

public class RolesRepository : Repository<BookRole, Guid>, IRolesRepository
{
    public RolesRepository(ApplicationDbContext dbContext, ILogger<Repository<BookRole, Guid>> logger) : base(dbContext, logger)
    {
    }
}