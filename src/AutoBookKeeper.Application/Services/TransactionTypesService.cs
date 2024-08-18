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

public class TransactionTypesService : ITransactionTypesService
{
    private readonly ITransactionTypesRepository _transactionTypesRepository;
    private readonly ILogger<TransactionTypesService> _logger;

    public TransactionTypesService(ITransactionTypesRepository transactionTypesRepository, ILogger<TransactionTypesService> logger)
    {
        _transactionTypesRepository = transactionTypesRepository;
        _logger = logger;
    }
    
    public async Task<IEnumerable<TransactionTypeModel>> GetAll()
    {
        var transactionTypes = await _transactionTypesRepository.GetAllAsync();
        return ApplicationMapper.Mapper.Map<IEnumerable<TransactionTypeModel>>(transactionTypes);
    }

    public async Task<IEnumerable<TransactionTypeModel>> GetBookTransactionTypes(Guid bookId)
    {
        var transactionTypes = await _transactionTypesRepository.GetAsync(TransactionTypeSpecification.GetBookTransactionTypes(bookId));
        return ApplicationMapper.Mapper.Map<IEnumerable<TransactionTypeModel>>(transactionTypes);
    }

    public async Task<TransactionTypeModel?> GetByIdAsync(Guid transactionTypeId)
    {
        var transactionType = await _transactionTypesRepository.GetByIdAsync(transactionTypeId);
        return ApplicationMapper.Mapper.Map<TransactionTypeModel>(transactionType);
    }

    public async Task<OperationResult<TransactionTypeModel>> CreateAsync(TransactionTypeModel transactionType)
    {
        // todo validate
        
        var result = await _transactionTypesRepository.CreateAsync(ApplicationMapper.Mapper.Map<TransactionType>(transactionType));

        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<TransactionTypeModel>> UpdateAsync(TransactionTypeModel transactionType)
    {
        var transactionTypeEntity = await _transactionTypesRepository.GetByIdAsync(transactionType.Id);

        if (transactionTypeEntity == null)
            return NotFoundResult();

        var updatedTransactionType = ApplicationMapper.Mapper.Map<TransactionType>(transactionType);
        updatedTransactionType.BookId = transactionTypeEntity.BookId;

        var result = await _transactionTypesRepository.UpdateAsync(updatedTransactionType);

        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<TransactionTypeModel>> DeleteAsync(TransactionTypeModel transactionType)
    {
        var transactionTypeEntity = await _transactionTypesRepository.GetByIdAsync(transactionType.Id);

        if (transactionTypeEntity == null)
            return NotFoundResult();
        
        var result = await _transactionTypesRepository.DeleteAsync(transactionTypeEntity);

        return MappedRepositoryResult(result);
    }
    
    private static OperationResult<TransactionTypeModel> MappedRepositoryResult(OperationResult<TransactionType> repositoryResult) => 
        repositoryResult.ToOperationResult(ApplicationMapper.Mapper.Map<TransactionTypeModel>);

    private static OperationResult<TransactionTypeModel> NotFoundResult() =>
        new () {Status = 404, Exception = new NotFoundException("Transaction type was not found")};
}