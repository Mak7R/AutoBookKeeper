using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoBookKeeper.Web.Filters;

public class ExceptionHandlingFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        if (exception is ArgumentException)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "Invalid Argument",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
        else
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        
        context.ExceptionHandled = true;
    }
}