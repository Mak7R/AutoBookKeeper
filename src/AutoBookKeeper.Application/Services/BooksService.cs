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

public class BooksService : IBooksService
{
    private readonly IBooksRepository _booksRepository;
    private readonly IValidator<BookModel> _bookValidator;
    private readonly ILogger<BooksService> _logger;

    public BooksService(IBooksRepository booksRepository, IValidator<BookModel> bookValidator, ILogger<BooksService> logger)
    {
        _booksRepository = booksRepository;
        _bookValidator = bookValidator;
        _logger = logger;
    }
    
    public async Task<IEnumerable<BookModel>> GetAll()
    {
        try
        {
            var books = await _booksRepository.GetAllAsync();
            return ApplicationMapper.Mapper.Map<IEnumerable<BookModel>>(books);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<IEnumerable<BookModel>> GetUserBooks(Guid userId)
    {
        try
        {
            var books = await _booksRepository.GetAsync(BookSpecification.GetUserBooks(userId));
            return ApplicationMapper.Mapper.Map<IEnumerable<BookModel>>(books);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<BookModel?> GetByIdAsync(Guid bookId)
    {
        try
        {
            var book = await _booksRepository.GetByIdAsync(bookId);
            return ApplicationMapper.Mapper.Map<BookModel>(book);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    private async Task<OperationResult<BookModel>?> ValidateAlreadyExists(Book book)
    {
        var books = await _booksRepository.GetAsync(BookSpecification.GetBuilder().ApplyOwner(book.Owner.Id).ApplyTitle(book.Title).Build());
        if (books.Any())
            return OperationResultExtensions.ValidationErrorResult<BookModel>(
                new Dictionary<string, string[]>
                {
                    { nameof(Book.Title), ["Book with this title already exists"] }, 
                });
        return null;
    }

    public async Task<OperationResult<BookModel>> CreateAsync(BookModel bookModel)
    {
        try
        {
            var validationResult = await _bookValidator.ValidateAsync(bookModel);
            if (!validationResult.IsValid)
                return OperationResultExtensions.ValidationErrorResult<BookModel>(validationResult);
            
            var bookEntity = ApplicationMapper.Mapper.Map<Book>(bookModel);
            bookEntity.CreationTime = DateTime.UtcNow;
            
            var alreadyExistsResult = await ValidateAlreadyExists(bookEntity);
            if (alreadyExistsResult != null) return alreadyExistsResult;
            
            var result = await _booksRepository.CreateAsync(bookEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<BookModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<BookModel>("An error occurred while creating the book");
        }
    }

    public async Task<OperationResult<BookModel>> UpdateAsync(BookModel bookModel)
    {
        try
        {
            var validationResult = await _bookValidator.ValidateAsync(bookModel);
            if (!validationResult.IsValid)
                return OperationResultExtensions.ValidationErrorResult<BookModel>(validationResult);
            
            var bookEntity = await _booksRepository.GetByIdAsync(bookModel.Id);
            if (bookEntity == null)
                return OperationResultExtensions.ErrorResult<BookModel>("Book was not found", 404);

            var updatedBookEntity = ApplicationMapper.Mapper.Map<Book>(bookModel);
            updatedBookEntity.OwnerId = bookEntity.OwnerId;
            updatedBookEntity.CreationTime = bookEntity.CreationTime;
            
            var alreadyExistsResult = await ValidateAlreadyExists(updatedBookEntity);
            if (alreadyExistsResult != null) return alreadyExistsResult;
            
            var result = await _booksRepository.UpdateAsync(updatedBookEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<BookModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<BookModel>("An error occurred while updating the book");
        }
    }

    public async Task<OperationResult<BookModel>> DeleteAsync(BookModel bookModel)
    {
        try
        {
            var bookEntity = await _booksRepository.GetByIdAsync(bookModel.Id);
            if (bookEntity == null)
                return OperationResultExtensions.ErrorResult<BookModel>("Book was not found", 404);

            var result = await _booksRepository.DeleteAsync(bookEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<BookModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<BookModel>("An error occurred while deleting the book");
        }
    }
}