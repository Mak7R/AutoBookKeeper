using AutoBookKeeper.Application.Helpers;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Specifications;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Services;

public class ForecastService : IForecastService
{
    private readonly IForecastProvider _forecastProvider;
    private readonly ILogger<ForecastService> _logger;

    public ForecastService(IForecastProvider forecastProvider, ILogger<ForecastService> logger)
    {
        _forecastProvider = forecastProvider;
        _logger = logger;
    }
    
    public async Task<Dictionary<DateTime, decimal>> PolynomialBalanceForecast(Guid bookId, DateTime endDate, int daysStep)
    {
        try
        {
            return await _forecastProvider.PolynomialBalanceForecast(TransactionSpecification.GetBookTransactions(bookId), endDate, daysStep);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }
}