using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;

public class TransactionTypesRepository : Repository<TransactionType, Guid>, ITransactionTypesRepository
{
    public TransactionTypesRepository(ApplicationDbContext dbContext, ILogger<Repository<TransactionType, Guid>> logger) : base(dbContext, logger)
    {
    }
}