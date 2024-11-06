using AutoBookKeeper.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AutoBookKeeper.Web.Extensions;

public static class ControllerBaseExtensions
{
    public static IActionResult ProblemResult(this ControllerBase controller, int statusCode, string detail, IDictionary<string,object?>? extensions = null)
    {
        var problem = new ProblemDetails
        {
            Detail = detail,
            Status = statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Instance = controller.Request.Path,
        };

        if (extensions != null)
            problem.Extensions = extensions;
        
        return controller.StatusCode(statusCode, problem);
    }

    public static IActionResult ProblemResult<T>(this ControllerBase controller, OperationResult<T> result)
    {
        if (!result.ValidationErrors.Any())
            return controller.Problem(detail: result.ErrorMessage, statusCode: result.Status);
        
        return controller.StatusCode(result.Status, new ProblemDetails
        {
            Detail = result.ErrorMessage,
            Status = result.Status,
            Title = ReasonPhrases.GetReasonPhrase(result.Status),
            Instance = controller.Request.Path,
            Extensions = new Dictionary<string, object?>{{"validationErrors", result.ValidationErrors}}
        });
    }
}