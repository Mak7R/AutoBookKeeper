using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class CalculationsController : ApiController
{
    private readonly ICalculationsService _calculationsService;

    public CalculationsController(ICalculationsService calculationsService)
    {
        _calculationsService = calculationsService;
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/sum")]
    public async Task<IActionResult> Sum(Guid bookId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(await _calculationsService.Sum(bookId, from, to));
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/balance")]
    public async Task<IActionResult> Balance(Guid bookId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(await _calculationsService.Balance(bookId, from, to));
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/balance-by-date")]
    public async Task<IActionResult> BalanceByDate(Guid bookId)
    {
        return Ok(await _calculationsService.BalanceByDate(bookId));
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/average-transaction")]
    public async Task<IActionResult> AverageTransaction(Guid bookId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(await _calculationsService.AverageTransaction(bookId, from, to));
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/max-transaction")]
    public async Task<IActionResult> MaxTransaction(Guid bookId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(await _calculationsService.MaxTransaction(bookId, from, to));
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/min-transaction")]
    public async Task<IActionResult> MinTransaction(Guid bookId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(await _calculationsService.MinTransaction(bookId, from, to));
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions/volatility")]
    public async Task<IActionResult> Volatility(Guid bookId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(await _calculationsService.Volatility(bookId, from, to));
    }
}