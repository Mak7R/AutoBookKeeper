using AutoBookKeeper.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Extensions;

public static class ControllerBaseExtensions
{
    public static IActionResult ProblemResult(this ControllerBase controller, int statusCode, string detail, string? title = null, IDictionary<string,object?>? extensions = null, string? instance = null, string? type = null)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Detail = detail,
            Title = string.IsNullOrEmpty(title) ? detail : title,
            Instance = instance,
            Type = type
        };

        if (extensions != null)
            problem.Extensions = extensions;
        
        return controller.StatusCode(statusCode, problem);
    }

    public static IActionResult ProblemResult<T>(this ControllerBase controller, OperationResult<T> result, string? title = null)
    {
        if (!result.Errors.Any())
            return controller.Problem(detail: result.Exception?.Message, statusCode: result.Status, title:title);
        
        if (result.Exception != null)
        {
            return controller.StatusCode(result.Status, new ProblemDetails
            {
                Status = result.Status,
                Title = title,
                Detail = result.Exception.Message,
                Extensions = new Dictionary<string, object?>{{"errors", result.Errors}}
            });
        }
        
        return controller.StatusCode(result.Status, new ProblemDetails
        {
            Status = result.Status,
            Title = title,
            Detail = result.Errors.FirstOrDefault().Value?.ToString(),
            Extensions = new Dictionary<string, object?>{{"errors", result.Errors}}
        });
    }
}