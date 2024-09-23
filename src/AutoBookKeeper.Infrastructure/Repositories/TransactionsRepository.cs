using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Repositories.Base;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;

public class TransactionsRepository : Repository<Transaction, Guid>, ITransactionsRepository
{
    public TransactionsRepository(ApplicationDbContext dbContext, ILogger<Repository<Transaction, Guid>> logger) : base(dbContext, logger)
    {
    }
}