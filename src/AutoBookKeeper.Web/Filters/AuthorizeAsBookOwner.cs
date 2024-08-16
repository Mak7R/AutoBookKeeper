using System.Security.Claims;
using AutoBookKeeper.Application.Interfaces;

namespace AutoBookKeeper.Web.Filters;

public class AuthorizeAsBookOwner : AuthorizeAsAttribute
{
    public AuthorizeAsBookOwner(string bookIdRoute) : base(GetValidator(bookIdRoute))
    {
    }
    
    private static Func<HttpContext, Task<bool>> GetValidator(string bookIdRoute)
    {
        return async context =>
        {
            var booksService = context.RequestServices.GetRequiredService<IBooksService>();
            
            var currentUserIdString =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var bookIdString = context.Request.RouteValues[bookIdRoute]?.ToString();

            if (!Guid.TryParse(bookIdString, out var bookId))
                return false;

            var book = await booksService.GetByIdAsync(bookId);
            
            if (string.Equals(currentUserIdString, book?.Owner.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))
                return true;
            
            return false;
        };
    }
}