using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Extensions;
using AutoBookKeeper.Web.Filters;
using AutoBookKeeper.Web.Models.Book;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class BooksController : ApiController
{
    private readonly IBooksService _booksService;
    private readonly IMapper _mapper;

    public BooksController(IBooksService booksService, IMapper mapper)
    {
        _booksService = booksService;
        _mapper = mapper;
    }

    [AuthorizeAsCurrentUser("userId")]
    [HttpGet("users/{userId:guid}/books")]
    public async Task<IActionResult> GetAll(Guid userId)
    {
        var books = await _booksService.GetUserBooks(userId);
        return Ok(_mapper.Map<IEnumerable<BookViewModel>>(books));
    }

    [AuthorizeAsCurrentUser("userId")]
    [HttpPost("users/{userId:guid}/books")]
    public async Task<IActionResult> Create(Guid userId, CreateBookDto createBookDto)
    {
        var book = _mapper.Map<BookModel>(createBookDto);
        book.Owner = new UserModel { Id = userId };
        
        var result = await _booksService.CreateAsync(book);
        
        if (result.IsSuccessful)
            return CreatedAtAction("Get", "Books", new {bookId = result.Result?.Id}, _mapper.Map<BookViewModel>(result.Result));
        
        return this.ProblemResult(result, "Create operation was not successful");
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}")]
    public async Task<IActionResult> Get(Guid bookId)
    {
        var book = await _booksService.GetByIdAsync(bookId);

        if (book == null)
            return Problem("Book was not found", statusCode: 404);
            
        return Ok(_mapper.Map<BookViewModel>(book));
    }

    [AuthorizeAsBookOwner("bookId")]
    [HttpPut("users/books/{bookId:guid}")]
    public async Task<IActionResult> Update(Guid bookId, UpdateBookDto updateBookDto)
    {
        var updateBook = _mapper.Map<BookModel>(updateBookDto);
        updateBook.Id = bookId;
        
        var result = await _booksService.UpdateAsync(updateBook);

        if (result.IsSuccessful)
            return Ok(_mapper.Map<BookViewModel>(result.Result));

        return this.ProblemResult(result, "Update operation was not successful");
    }

    [AuthorizeAsBookOwner("bookId")]
    [HttpDelete("users/books/{bookId:guid}")]
    public async Task<IActionResult> Delete(Guid bookId)
    {
        var result = await _booksService.DeleteAsync(new BookModel { Id = bookId });

        if (result.IsSuccessful)
            return Ok(_mapper.Map<BookViewModel>(result.Result));

        return this.ProblemResult(result, "Delete operation was not successful");
    }
}