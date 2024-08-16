using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface ITransactionsService
{
    Task<IEnumerable<TransactionModel>> GetAll();
    Task<IEnumerable<TransactionModel>> GetBookTransactions(Guid bookId);
    Task<TransactionModel?> GetByIdAsync(Guid transactionId);
    Task<OperationResult<TransactionModel>> CreateAsync(TransactionModel transaction);
    Task<OperationResult<TransactionModel>> UpdateAsync(TransactionModel transaction);
    Task<OperationResult<TransactionModel>> DeleteAsync(TransactionModel transaction);
}