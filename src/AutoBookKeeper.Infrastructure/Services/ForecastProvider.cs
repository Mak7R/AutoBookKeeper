using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using MathNet.Numerics;
using Microsoft.EntityFrameworkCore;

namespace AutoBookKeeper.Infrastructure.Services;

public class ForecastProvider : IForecastProvider
{
    private readonly ApplicationDbContext _dbContext;

    public ForecastProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IQueryable<Transaction> GetTransactionsQuery(ISpecification<Transaction> specification)
    {
        return SpecificationEvaluator<Transaction, Guid>.GetQuery(
            _dbContext.Transactions.AsNoTracking().AsQueryable(), specification);
    }
    
    public async Task<Dictionary<DateTime, decimal>> PolynomialBalanceForecast(ISpecification<Transaction> specification, DateTime endDate, int daysStep, int degree = 4)
    {
        var today = DateTime.Today;
        if (endDate <= today || (endDate - today).Days < daysStep)
            return new Dictionary<DateTime, decimal>();
        
        var query = GetTransactionsQuery(specification);
        var transactions = await query.OrderBy(t => t.TransactionTime).ToListAsync();

        if (transactions.Count == 0) return new Dictionary<DateTime, decimal>();
        
        var balanceByDate = new Dictionary<DateTime, decimal>();
        decimal cumulativeBalance = 0;

        foreach (var transaction in transactions)
        {
            cumulativeBalance += transaction.Value;
            var date = transaction.TransactionTime.Date;
            balanceByDate[date] = cumulativeBalance;
        }
        
        var xData = balanceByDate.Keys.Select(d => d.ToOADate()).ToArray();
        var yData = balanceByDate.Values.Select(b => (double)b).ToArray();

        var coefficients = Fit.Polynomial(xData, yData, degree);

        var forecastResults = new Dictionary<DateTime, decimal>();
        for (var currentDate = today; currentDate <= endDate; currentDate = currentDate.AddDays(daysStep))
        {
            double forecastX = currentDate.ToOADate();
            double forecastY = 0;
            
            for (int i = 0; i <= degree; i++)
            {
                forecastY += coefficients[i] * Math.Pow(forecastX, i);
            }
            
            forecastResults[currentDate] = (decimal)forecastY;
        }

        return forecastResults;
    }
}