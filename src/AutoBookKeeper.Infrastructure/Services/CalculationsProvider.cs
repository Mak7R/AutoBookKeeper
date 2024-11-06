using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Helpers;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Services;

public class CalculationsProvider : ICalculationsProvider
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CalculationsProvider> _logger;

    public CalculationsProvider(ApplicationDbContext dbContext, ILogger<CalculationsProvider> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    private IQueryable<Transaction> GetTransactionsQuery(ISpecification<Transaction> specification)
    {
        return SpecificationEvaluator<Transaction, Guid>.GetQuery(
            _dbContext.Transactions.AsNoTracking().AsQueryable(), specification);
    }

    public async Task<int> Count(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await query.CountAsync();
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
    
    public async Task<decimal> Sum(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await query.SumAsync(t => t.Value < 0 ? -t.Value : t.Value);
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
    
    public async Task<decimal> Balance(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await query.SumAsync(t => t.Value);
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public async Task<Dictionary<DateTime, decimal>> BalanceByDate(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            var transactions = await query.OrderBy(t => t.TransactionTime).ToListAsync();

            var balanceByDate = new Dictionary<DateTime, decimal>();
            decimal cumulativeBalance = 0;

            foreach (var transaction in transactions)
            {
                cumulativeBalance += transaction.Value;
                var date = transaction.TransactionTime.Date;
                balanceByDate[date] = cumulativeBalance;
            }

            return balanceByDate;
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
    
    public async Task<decimal> AverageTransaction(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await query.AverageAsync(t => t.Value);
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public async Task<decimal> MaxTransaction(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await query.MaxAsync(t => t.Value);
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public async Task<decimal> MinTransaction(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await query.MinAsync(t => t.Value);
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
    
    public async Task<decimal> Volatility(ISpecification<Transaction> specification)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            var values = await query.Select(t => t.Value).ToListAsync();
            var average = values.Average();
            var sumOfSquaresOfDifferences = values.Select(v => (v - average) * (v - average)).Sum();
            return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / values.Count));
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public async Task<T> Calculate<T>(ISpecification<Transaction> specification, Func<IQueryable<Transaction>, Task<T>> calculateFunction)
    {
        try
        {
            var query = GetTransactionsQuery(specification);
            return await calculateFunction(query);
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
}