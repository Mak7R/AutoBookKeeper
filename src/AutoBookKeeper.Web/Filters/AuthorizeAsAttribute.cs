using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoBookKeeper.Web.Filters;


public abstract class AuthorizeAsAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly Func<HttpContext, Task<bool>> _validator;
    
    protected AuthorizeAsAttribute(Func<HttpContext, Task<bool>> validator)
    {
        _validator = validator;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!await _validator(context.HttpContext))
        {
            context.Result = new ForbidResult();
        }
    }
}

