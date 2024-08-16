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

public class BooksService : IBooksService
{
    private readonly IBooksRepository _booksRepository;
    private readonly ILogger<BooksService> _logger;

    public BooksService(IBooksRepository booksRepository, ILogger<BooksService> logger)
    {
        _booksRepository = booksRepository;
        _logger = logger;
    }
    
    public async Task<IEnumerable<BookModel>> GetAll()
    {
        var books = await _booksRepository.GetAllAsync();
        return ApplicationMapper.Mapper.Map<IEnumerable<BookModel>>(books);
    }

    public async Task<IEnumerable<BookModel>> GetUserBooks(Guid userId)
    {
        var books = await _booksRepository.GetAsync(BookSpecification.GetUserBooks(userId));
        return ApplicationMapper.Mapper.Map<IEnumerable<BookModel>>(books);
    }

    public async Task<BookModel?> GetByIdAsync(Guid bookId)
    {
        var book = await _booksRepository.GetByIdAsync(bookId);
        return ApplicationMapper.Mapper.Map<BookModel>(book);
    }

    public async Task<OperationResult<BookModel>> CreateAsync(BookModel book)
    {
        book.CreationTime = DateTime.UtcNow;
        
        // todo validate
        
        var result = await _booksRepository.CreateAsync(ApplicationMapper.Mapper.Map<Book>(book));

        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<BookModel>> UpdateAsync(BookModel book)
    {
        var bookEntity = await _booksRepository.GetByIdAsync(book.Id);

        if (bookEntity == null)
            return NotFoundResult();

        var updatedBook = ApplicationMapper.Mapper.Map<Book>(book);
        updatedBook.OwnerId = bookEntity.OwnerId;
        updatedBook.CreationTime = bookEntity.CreationTime;

        var result = await _booksRepository.UpdateAsync(updatedBook);

        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<BookModel>> DeleteAsync(BookModel book)
    {
        var bookEntity = await _booksRepository.GetByIdAsync(book.Id);

        if (bookEntity == null)
            return NotFoundResult();
        
        var result = await _booksRepository.DeleteAsync(bookEntity);

        return MappedRepositoryResult(result);
    }
    
    private static OperationResult<BookModel> MappedRepositoryResult(OperationResult<Book> repositoryResult) => 
        repositoryResult.ToOperationResult(ApplicationMapper.Mapper.Map<BookModel>);
    
    private static OperationResult<BookModel> NotFoundResult(params string[] errors) => 
        errors.Length > 0 ? 
            new OperationResult<BookModel> {Status = 404, Exception = new NotFoundException(errors[0]), Errors = errors} : 
            new OperationResult<BookModel> {Status = 404, Exception = new NotFoundException("Book was not found"), Errors = ["Book was not found"]};

    private static OperationResult<BookModel> AlreadyExistsResult(params string[] errors) => 
        errors.Length > 0 ? 
            new OperationResult<BookModel> {Status = 400, Exception = new AlreadyExistsException(errors[0]), Errors = errors} : 
            new OperationResult<BookModel> {Status = 400, Exception = new AlreadyExistsException("Book already exists"), Errors = ["Book already exists"]};
}