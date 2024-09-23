using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Specifications;

namespace AutoBookKeeper.Application.Services;

public class CalculationsService : ICalculationsService
{
    private readonly ICalculationsProvider _calculationsProvider;

    public CalculationsService(ICalculationsProvider calculationsProvider)
    {
        _calculationsProvider = calculationsProvider;
    }

    public async Task<decimal> Sum(Guid bookId, DateTime? from, DateTime? to)
    {
        return await _calculationsProvider.Sum(
            TransactionSpecification
                .GetBuilder()
                .ApplyBook(bookId)
                .ApplyDataTimeRange(from, to)
                .Build()
            );
    }

    public async Task<decimal> Balance(Guid bookId, DateTime? from, DateTime? to)
    {
        return await _calculationsProvider.Balance(
            TransactionSpecification
                .GetBuilder()
                .ApplyBook(bookId)
                .ApplyDataTimeRange(from, to)
                .Build()
            );
    }

    public async Task<Dictionary<DateTime, decimal>> BalanceByDate(Guid bookId)
    {
        return await _calculationsProvider.BalanceByDate(TransactionSpecification.GetBookTransactions(bookId));
    }

    public async Task<decimal> AverageTransaction(Guid bookId, DateTime? from, DateTime? to)
    {
        return await _calculationsProvider.AverageTransaction(
            TransactionSpecification
                .GetBuilder()
                .ApplyBook(bookId)
                .ApplyDataTimeRange(from, to)
                .Build()
        );
    }

    public async Task<decimal> MaxTransaction(Guid bookId, DateTime? from, DateTime? to)
    {
        return await _calculationsProvider.MaxTransaction(
            TransactionSpecification
                .GetBuilder()
                .ApplyBook(bookId)
                .ApplyDataTimeRange(from, to)
                .Build()
        );
    }

    public async Task<decimal> MinTransaction(Guid bookId, DateTime? from, DateTime? to)
    {
        return await _calculationsProvider.MinTransaction(
            TransactionSpecification
                .GetBuilder()
                .ApplyBook(bookId)
                .ApplyDataTimeRange(from, to)
                .Build()
        );
    }

    public async Task<decimal> Volatility(Guid bookId, DateTime? from, DateTime? to)
    {
        return await _calculationsProvider.Volatility(
            TransactionSpecification
                .GetBuilder()
                .ApplyBook(bookId)
                .ApplyDataTimeRange(from, to)
                .Build()
        );
    }
}