using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Extensions;
using AutoBookKeeper.Web.Filters;
using AutoBookKeeper.Web.Models.Transaction;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class TransactionsController : ApiController
{
    private readonly ITransactionsService _transactionsService;
    private readonly IMapper _mapper;

    public TransactionsController(ITransactionsService transactionsService, IMapper mapper)
    {
        _transactionsService = transactionsService;
        _mapper = mapper;
    }
    
    [AuthorizeAsBookOwner("bookId")]
    [HttpGet("users/books/{bookId:guid}/transactions")]
    public async Task<IActionResult> GetAll(Guid bookId)
    {
        var transactions = await _transactionsService.GetBookTransactions(bookId);
        return Ok(_mapper.Map<IEnumerable<TransactionViewModel>>(transactions));
    }

    [AuthorizeAsBookOwner("bookId")]
    [HttpPost("users/books/{bookId:guid}/transactions")]
    public async Task<IActionResult> Create(Guid bookId, CreateTransactionDto createTransactionDto)
    {
        var transaction = _mapper.Map<TransactionModel>(createTransactionDto);
        transaction.Book = new BookModel{Id = bookId};
        
        var result = await _transactionsService.CreateAsync(transaction);
        
        if (result.IsSuccessful)
            return CreatedAtAction("Get", "Transactions", new {transactionId = result.Result?.Id}, _mapper.Map<TransactionViewModel>(result.Result));
        
        return this.ProblemResult(result, "Create operation was not successful");
    }
    
    [AuthorizeAsTransactionOwner("transactionId")]
    [HttpGet("users/books/transactions/{transactionId:guid}")]
    public async Task<IActionResult> Get(Guid transactionId)
    {
        var transaction = await _transactionsService.GetByIdAsync(transactionId);

        if (transaction == null)
            return Problem("Transaction was not found", statusCode: 404);
            
        return Ok(_mapper.Map<TransactionViewModel>(transaction));
    }

    [AuthorizeAsTransactionOwner("transactionId")]
    [HttpPut("users/books/transactions/{transactionId:guid}")] 
    public async Task<IActionResult> Update(Guid transactionId, UpdateTransactionDto updateTransactionDto)
    {
        var updateTransaction = _mapper.Map<TransactionModel>(updateTransactionDto);
        updateTransaction.Id = transactionId;
        
        var result = await _transactionsService.UpdateAsync(updateTransaction);

        if (result.IsSuccessful)
            return Ok(_mapper.Map<TransactionViewModel>(result.Result));

        return this.ProblemResult(result, "Update operation was not successful");
    }

    [AuthorizeAsTransactionOwner("transactionId")]
    [HttpDelete("users/books/transactions/{transactionId:guid}")]
    public async Task<IActionResult> Delete(Guid transactionId)
    {
        var result = await _transactionsService.DeleteAsync(new TransactionModel{Id = transactionId});

        if (result.IsSuccessful)
            return Ok(_mapper.Map<TransactionViewModel>(result.Result));

        return this.ProblemResult(result, "Delete operation was not successful");
    }
}