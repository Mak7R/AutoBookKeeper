using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Application.Interfaces;

public interface IForecastProvider
{
    Task<Dictionary<DateTime, decimal>> PolynomialBalanceForecast(ISpecification<Transaction> specification, DateTime endDate, int daysStep, int degree = 2);
}