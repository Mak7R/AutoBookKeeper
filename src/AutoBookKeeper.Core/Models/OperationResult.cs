
using System.Collections;

namespace AutoBookKeeper.Core.Models;

public class OperationResult : OperationResult<object>;

public class OperationResult<T>
{
    public bool IsSuccessful => Status is >= 200 and < 300;
    public int Status { get; init; } = 200;
    public T? Result { get; init; }
    public Exception? Exception { get; init; }
    public IDictionary<string, IEnumerable<object>> Errors { get; init; } = new Dictionary<string, IEnumerable<object>>();

    public OperationResult()
    {
        
    }
    
    public OperationResult(IDictionary<string, IEnumerable<object>> errors)
    {
        Errors = errors;
    }
    
    public OperationResult<T> WithError(string key, IEnumerable<object> value)
    {
        Errors[key] = value;
        return this;
    }
    
    public static OperationResult<T> Ok(T? result) => 
        new () { Status = 200, Result = result };
    
    public static OperationResult<T> ServerError(Exception ex) =>
        new () { Status = 500, Exception = ex };
}

public static class OperationResultExtensions
{
    
    
    public static OperationResult<T> WithErrors<T>(this OperationResult<T> result, IDictionary<string, IEnumerable<object>> errors)
    {
        foreach (var error in errors)
            result.WithError(error.Key, error.Value);

        return result;
    }
    
    public static OperationResult<TDest> ToOperationResult<TSource, TDest>(this OperationResult<TSource> src, TDest? result = default)
    {
        return new OperationResult<TDest>
            { Status = src.Status, Exception = src.Exception, Result = result, Errors = src.Errors };
    }
    
    public static OperationResult<TDest> ToOperationResult<TSource, TDest>(this OperationResult<TSource> operationResult, Func<TSource?, TDest?> mapper) =>
        operationResult.ToOperationResult(mapper(operationResult.Result));
}