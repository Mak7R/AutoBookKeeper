using System.Security.Claims;
using AutoBookKeeper.Application.Interfaces;

namespace AutoBookKeeper.Web.Filters;

public class AuthorizeAsTransactionOwner : AuthorizeAsAttribute
{
    public AuthorizeAsTransactionOwner(string transactionIdRoute) : base(GetValidator(transactionIdRoute))
    {
    }
    
    private static Func<HttpContext, Task<bool>> GetValidator(string transactionIdRoute)
    {
        return async context =>
        {
            var transactionService = context.RequestServices.GetRequiredService<ITransactionsService>();
            var booksService = context.RequestServices.GetRequiredService<IBooksService>();
            
            var currentUserIdString =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var transactionIdString = context.Request.RouteValues[transactionIdRoute]?.ToString();

            if (!Guid.TryParse(transactionIdString, out var transactionId))
                return false;

            // todo 1 request with includes

            var transaction = await transactionService.GetByIdAsync(transactionId);
            if (transaction == null)
                return false;
            
            var book = await booksService.GetByIdAsync(transaction.Book.Id);
            
            if (string.Equals(currentUserIdString, book?.Owner.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))
                return true;
            
            return false;
        };
    }
}