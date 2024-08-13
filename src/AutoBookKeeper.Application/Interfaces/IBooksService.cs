using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IBooksService
{
    Task<IEnumerable<BookModel>> GetAll();
    Task<IEnumerable<BookModel>> GetUserBooks(Guid userId);
    Task<IEnumerable<BookModel>> GetAvailableForUserBooks(Guid userId);
    Task<BookModel?> GetByIdAsync(Guid bookId);
    Task<OperationResult<BookModel>> CreateAsync(BookModel book);
    Task<OperationResult<BookModel>> UpdateAsync(BookModel book);
    Task<OperationResult<BookModel>> DeleteAsync(BookModel book);
}