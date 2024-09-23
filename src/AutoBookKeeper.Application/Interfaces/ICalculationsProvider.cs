using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Application.Interfaces;

public interface ICalculationsProvider
{
    Task<int> Count(ISpecification<Transaction> specification);
    Task<decimal> Sum(ISpecification<Transaction> specification);
    Task<decimal> Balance(ISpecification<Transaction> specification);
    Task<Dictionary<DateTime, decimal>> BalanceByDate(ISpecification<Transaction> specification);
    Task<decimal> AverageTransaction(ISpecification<Transaction> specification);
    Task<decimal> MaxTransaction(ISpecification<Transaction> specification);
    Task<decimal> MinTransaction(ISpecification<Transaction> specification);
    Task<decimal> Volatility(ISpecification<Transaction> specification);
    Task<T> Calculate<T>(ISpecification<Transaction> specification, Func<IQueryable<Transaction>, Task<T>> calculateFunction);
}