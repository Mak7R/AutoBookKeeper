using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IBooksService
{
    Task<IEnumerable<BookModel>> GetAll();
    Task<IEnumerable<BookModel>> GetUserBooks(Guid userId);
    Task<BookModel?> GetByIdAsync(Guid bookId);
    Task<OperationResult<BookModel>> CreateAsync(BookModel bookModel);
    Task<OperationResult<BookModel>> UpdateAsync(BookModel bookModel);
    Task<OperationResult<BookModel>> DeleteAsync(BookModel bookModel);
}