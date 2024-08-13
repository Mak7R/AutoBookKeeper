using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Mappers;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Specifications;

namespace AutoBookKeeper.Application.Services;

public class BooksService : IBooksService
{
    private readonly IBooksRepository _booksRepository;

    public BooksService(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }


    public Task<IEnumerable<BookModel>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BookModel>> GetUserBooks(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BookModel>> GetAvailableForUserBooks(Guid userId)
    {
        var books = await _booksRepository.GetAsync(new BookSpecification(b => b.Owner.Id == userId));
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
        // todo validation checks
        var result = await _booksRepository.CreateAsync(ApplicationMapper.Mapper.Map<Book>(book));

        return ParseRepositoryResult(result);
    }

    public Task<OperationResult<BookModel>> UpdateAsync(BookModel book)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<BookModel>> DeleteAsync(BookModel book)
    {
        throw new NotImplementedException();
    }
    
    private static OperationResult<BookModel> ParseRepositoryResult(OperationResult<Book> repositoryResult) => 
        repositoryResult.ToOperationResult(ApplicationMapper.Mapper.Map<BookModel>);
}