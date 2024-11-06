using AutoBookKeeper.Application.Helpers;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Specifications;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Services;

public class CalculationsService : ICalculationsService
{
    private readonly ICalculationsProvider _calculationsProvider;
    private readonly ILogger<CalculationsService> _logger;

    public CalculationsService(ICalculationsProvider calculationsProvider, ILogger<CalculationsService> logger)
    {
        _calculationsProvider = calculationsProvider;
        _logger = logger;
    }

    public async Task<decimal> Sum(Guid bookId, DateTime? from, DateTime? to)
    {
        try
        {
            return await _calculationsProvider.Sum(
                TransactionSpecification
                    .GetBuilder()
                    .ApplyBook(bookId)
                    .ApplyDataTimeRange(from, to)
                    .Build()
            );
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<decimal> Balance(Guid bookId, DateTime? from, DateTime? to)
    {
        try
        {
            return await _calculationsProvider.Balance(
                TransactionSpecification
                    .GetBuilder()
                    .ApplyBook(bookId)
                    .ApplyDataTimeRange(from, to)
                    .Build()
            );
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<Dictionary<DateTime, decimal>> BalanceByDate(Guid bookId)
    {
        return await _calculationsProvider.BalanceByDate(TransactionSpecification.GetBookTransactions(bookId));
    }

    public async Task<decimal> AverageTransaction(Guid bookId, DateTime? from, DateTime? to)
    {
        try
        {
            return await _calculationsProvider.AverageTransaction(
                        TransactionSpecification
                            .GetBuilder()
                            .ApplyBook(bookId)
                            .ApplyDataTimeRange(from, to)
                            .Build()
                    );
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<decimal> MaxTransaction(Guid bookId, DateTime? from, DateTime? to)
    {
        try
        {
            return await _calculationsProvider.MaxTransaction(
                        TransactionSpecification
                            .GetBuilder()
                            .ApplyBook(bookId)
                            .ApplyDataTimeRange(from, to)
                            .Build()
                    );
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<decimal> MinTransaction(Guid bookId, DateTime? from, DateTime? to)
    {
        try
        {
            return await _calculationsProvider.MinTransaction(
                        TransactionSpecification
                            .GetBuilder()
                            .ApplyBook(bookId)
                            .ApplyDataTimeRange(from, to)
                            .Build()
                    );
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<decimal> Volatility(Guid bookId, DateTime? from, DateTime? to)
    {
        try
        {
            return await _calculationsProvider.Volatility(
                        TransactionSpecification
                            .GetBuilder()
                            .ApplyBook(bookId)
                            .ApplyDataTimeRange(from, to)
                            .Build()
                    );
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }
}