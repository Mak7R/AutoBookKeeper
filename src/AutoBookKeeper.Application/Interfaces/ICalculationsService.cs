namespace AutoBookKeeper.Application.Interfaces;

public interface ICalculationsService
{
    Task<decimal> Sum(Guid bookId, DateTime? from, DateTime? to);
    Task<decimal> Balance(Guid bookId, DateTime? from, DateTime? to);
    Task<Dictionary<DateTime, decimal>> BalanceByDate(Guid bookId);
    Task<decimal> AverageTransaction(Guid bookId, DateTime? from, DateTime? to);
    Task<decimal> MaxTransaction(Guid bookId, DateTime? from, DateTime? to);
    Task<decimal> MinTransaction(Guid bookId, DateTime? from, DateTime? to);
    Task<decimal> Volatility(Guid bookId, DateTime? from, DateTime? to);
}