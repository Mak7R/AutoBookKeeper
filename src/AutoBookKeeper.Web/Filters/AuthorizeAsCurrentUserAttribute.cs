using System.Security.Claims;

namespace AutoBookKeeper.Web.Filters;

public class AuthorizeAsCurrentUserAttribute : AuthorizeAsAttribute
{
    public AuthorizeAsCurrentUserAttribute(string userIdRoute) : base(GetValidator(userIdRoute))
    {
    }

    private static Func<HttpContext, Task<bool>> GetValidator(string userIdRoute)
    {
        return context =>
        {
            var currentUserIdString =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userIdString = context.Request.RouteValues[userIdRoute]?.ToString();

            if (!string.Equals(currentUserIdString, userIdString, StringComparison.CurrentCultureIgnoreCase))
                return Task.FromResult(false);

            return Task.FromResult(true);
        };
    }
}