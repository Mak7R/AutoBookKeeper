using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface ITransactionsService
{
    Task<IEnumerable<TransactionModel>> GetAll();
    Task<IEnumerable<TransactionModel>> GetBookTransactions(Guid bookId);
    Task<TransactionModel?> GetByIdAsync(Guid transactionId);
    Task<OperationResult<TransactionModel>> CreateAsync(TransactionModel transactionModel);
    Task<OperationResult<TransactionModel>> UpdateAsync(TransactionModel transactionModel);
    Task<OperationResult<TransactionModel>> DeleteAsync(TransactionModel transactionModel);
}