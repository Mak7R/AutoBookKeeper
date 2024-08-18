using System.Security.Cryptography;
using AutoBookKeeper.Application.Exceptions;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Mappers;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Specifications;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Services;


public class TransactionsService : ITransactionsService
{
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly ILogger<TransactionsService> _logger;

    public TransactionsService(ITransactionsRepository transactionsRepository, ILogger<TransactionsService> logger)
    {
        _transactionsRepository = transactionsRepository;
        _logger = logger;
    }
    
    public async Task<IEnumerable<TransactionModel>> GetAll()
    {
        var transactions = await _transactionsRepository.GetAllAsync();
        return ApplicationMapper.Mapper.Map<IEnumerable<TransactionModel>>(transactions);
    }

    public async Task<IEnumerable<TransactionModel>> GetBookTransactions(Guid bookId)
    {
        var transactions = await _transactionsRepository.GetAsync(TransactionSpecification.GetBookTransactions(bookId));
        return ApplicationMapper.Mapper.Map<IEnumerable<TransactionModel>>(transactions);
    }

    public async Task<TransactionModel?> GetByIdAsync(Guid transactionId)
    {
        var transaction = await _transactionsRepository.GetByIdAsync(transactionId);
        return ApplicationMapper.Mapper.Map<TransactionModel>(transaction);
    }

    public async Task<OperationResult<TransactionModel>> CreateAsync(TransactionModel transaction)
    {
        var transactionEntity = ApplicationMapper.Mapper.Map<Transaction>(transaction);
        
        if (string.IsNullOrEmpty(transactionEntity.NameIdentifier))
            transactionEntity.NameIdentifier = GenerateDefaultNameIdentifier(transactionEntity);
        
        var result = await _transactionsRepository.CreateAsync(transactionEntity);

        return MappedRepositoryResult(result);
    }

    private const string RandomNameIdChoices = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int RandomNameIdLength = 6;
    private static string GenerateDefaultNameIdentifier(Transaction transaction) =>
        $"{transaction.TransactionTime:dd-MM-yyyy|HH:mm:ss}|{RandomNumberGenerator.GetString(RandomNameIdChoices, RandomNameIdLength)}";

    public async Task<OperationResult<TransactionModel>> UpdateAsync(TransactionModel transaction)
    {
        var transactionEntity = await _transactionsRepository.GetByIdAsync(transaction.Id);

        if (transactionEntity == null)
            return NotFoundResult();

        var updatedTransaction = ApplicationMapper.Mapper.Map<Transaction>(transaction);
        updatedTransaction.BookId = transactionEntity.BookId;

        var result = await _transactionsRepository.UpdateAsync(updatedTransaction);

        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<TransactionModel>> DeleteAsync(TransactionModel transaction)
    {
        var transactionEntity = await _transactionsRepository.GetByIdAsync(transaction.Id);

        if (transactionEntity == null)
            return NotFoundResult();
        
        var result = await _transactionsRepository.DeleteAsync(transactionEntity);

        return MappedRepositoryResult(result);
    }
    
    private static OperationResult<TransactionModel> MappedRepositoryResult(OperationResult<Transaction> repositoryResult) => 
        repositoryResult.ToOperationResult(ApplicationMapper.Mapper.Map<TransactionModel>);
    
    private static OperationResult<TransactionModel> NotFoundResult() => 
        new () {Status = 404, Exception = new NotFoundException("Transaction was not found")};
}