using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Application.Interfaces;

public interface IForecastService
{
    Task<Dictionary<DateTime, decimal>> PolynomialBalanceForecast(Guid bookId, DateTime endDate, int daysStep);
}