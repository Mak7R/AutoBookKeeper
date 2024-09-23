using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Specifications;

namespace AutoBookKeeper.Application.Services;

public class ForecastService : IForecastService
{
    private readonly IForecastProvider _forecastProvider;

    public ForecastService(IForecastProvider forecastProvider)
    {
        _forecastProvider = forecastProvider;
    }
    
    public async Task<Dictionary<DateTime, decimal>> PolynomialBalanceForecast(Guid bookId, DateTime endDate, int daysStep)
    {
        return await _forecastProvider.PolynomialBalanceForecast(TransactionSpecification.GetBookTransactions(bookId), endDate, daysStep);
    }
}