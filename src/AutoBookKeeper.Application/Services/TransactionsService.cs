using System.Security.Cryptography;
using AutoBookKeeper.Application.Extensions;
using AutoBookKeeper.Application.Helpers;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Mappers;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Specifications;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Services;


public class TransactionsService : ITransactionsService
{
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly IValidator<TransactionModel> _transactionValidator;
    private readonly ILogger<TransactionsService> _logger;

    public TransactionsService(ITransactionsRepository transactionsRepository, IValidator<TransactionModel> transactionValidator, ILogger<TransactionsService> logger)
    {
        _transactionsRepository = transactionsRepository;
        _transactionValidator = transactionValidator;
        _logger = logger;
    }
    
    public async Task<IEnumerable<TransactionModel>> GetAll()
    {
        try
        {
            var transactions = await _transactionsRepository.GetAllAsync();
            return ApplicationMapper.Mapper.Map<IEnumerable<TransactionModel>>(transactions);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<IEnumerable<TransactionModel>> GetBookTransactions(Guid bookId)
    {
        try
        {
            var transactions = await _transactionsRepository.GetAsync(TransactionSpecification.GetBookTransactions(bookId));
            return ApplicationMapper.Mapper.Map<IEnumerable<TransactionModel>>(transactions);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<TransactionModel?> GetByIdAsync(Guid transactionId)
    {
        try
        {
            var transaction = await _transactionsRepository.GetByIdAsync(transactionId);
            return ApplicationMapper.Mapper.Map<TransactionModel>(transaction);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    private async Task<OperationResult<TransactionModel>?> ValidateAlreadyExists(Transaction transaction)
    {
        var transactions = await _transactionsRepository.GetAsync(
            TransactionSpecification.GetBuilder()
                .ApplyBook(transaction.Book.Id)
                .ApplyNameIdentifier(transaction.NameIdentifier)
                .Build());
            
        if (transactions.Any())
            return OperationResultExtensions.ValidationErrorResult<TransactionModel>(
                new Dictionary<string, string[]>
                {
                    { nameof(Transaction.NameIdentifier), ["Transaction with this name identifier already exists"] }, 
                });
        return null;
    }

    public async Task<OperationResult<TransactionModel>> CreateAsync(TransactionModel transactionModel)
    {
        try
        {
            var validationResult = await _transactionValidator.ValidateAsync(transactionModel);
            if (!validationResult.IsValid)
                return OperationResultExtensions.ValidationErrorResult<TransactionModel>(validationResult);
            
            var transactionEntity = ApplicationMapper.Mapper.Map<Transaction>(transactionModel);
        
            if (string.IsNullOrEmpty(transactionEntity.NameIdentifier))
                transactionEntity.NameIdentifier = GenerateDefaultNameIdentifier(transactionEntity);
            else
            {
                var alreadyExistsResult = await ValidateAlreadyExists(transactionEntity);
                if (alreadyExistsResult != null) return alreadyExistsResult;
            }
            
            var result = await _transactionsRepository.CreateAsync(transactionEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<TransactionModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<TransactionModel>("An error occurred while creating the transaction");
        }
    }

    private const string RandomNameIdChoices = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int RandomNameIdLength = 6;
    private static string GenerateDefaultNameIdentifier(Transaction transaction) =>
        $"{transaction.TransactionTime:dd-MM-yyyy|HH:mm:ss}|{RandomNumberGenerator.GetString(RandomNameIdChoices, RandomNameIdLength)}";

    public async Task<OperationResult<TransactionModel>> UpdateAsync(TransactionModel transactionModel)
    {
        try
        {
            var validationResult = await _transactionValidator.ValidateAsync(transactionModel);
            if (!validationResult.IsValid)
                return OperationResultExtensions.ValidationErrorResult<TransactionModel>(validationResult);
            
            var transactionEntity = await _transactionsRepository.GetByIdAsync(transactionModel.Id);
            if (transactionEntity == null)
                return OperationResultExtensions.ErrorResult<TransactionModel>("Transaction was not found", 404);
            
            var updatedTransactionEntity = ApplicationMapper.Mapper.Map<Transaction>(transactionModel);
            updatedTransactionEntity.BookId = transactionEntity.BookId;
            
            var alreadyExistsResult = await ValidateAlreadyExists(updatedTransactionEntity);
            if (alreadyExistsResult != null) return alreadyExistsResult;
            
            var result = await _transactionsRepository.UpdateAsync(updatedTransactionEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<TransactionModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<TransactionModel>("An error occurred while updating the transaction");
        }
    }

    public async Task<OperationResult<TransactionModel>> DeleteAsync(TransactionModel transactionModel)
    {
        try
        {
            var transactionEntity = await _transactionsRepository.GetByIdAsync(transactionModel.Id);
            if (transactionEntity == null)
                return OperationResultExtensions.ErrorResult<TransactionModel>("Transaction was not found", 404);
        
            var result = await _transactionsRepository.DeleteAsync(transactionEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<TransactionModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<TransactionModel>("An error occurred while deleting the transaction");
        }
    }
}