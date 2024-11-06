using AutoBookKeeper.Core.Models;
using FluentValidation.Results;

namespace AutoBookKeeper.Application.Extensions;

public static class OperationResultExtensions
{
    public static OperationResult<T> OkResult<T>(T result, int status = 200) =>
        new(status) { Result = result };

    public static OperationResult<T> ValidationErrorResult<T>(IDictionary<string, string[]> errors, int status = 400) =>
        new(status) { ValidationErrors = errors };

    public static OperationResult<T> ValidationErrorResult<T>(ValidationResult validationResult, int status = 400)
    {
        return ValidationErrorResult<T>(validationResult.ToDictionary(), status);
    }
    
    public static OperationResult<T> ErrorResult<T>(string message, int status = 500) =>
        new (status) { ErrorMessage = message };
}