
namespace AutoBookKeeper.Core.Models;

public class OperationResult : OperationResult<object>;

public class OperationResult<T>
{
    public bool IsSuccessful => Status is >= 200 and < 300;
    public int Status { get; init; }
    public T? Result { get; init; } = default;
    public Exception? Exception { get; init; } = null;
    public IEnumerable<string> Errors { get; init; } = [];

    public static OperationResult<T> FromStatus(int status) => 
        new() { Status = status };

    public static OperationResult<T> FromResult(T? result) => 
        new() { Status = 200, Result = result };
    public static OperationResult<T> FromResult(int status, T? result) => 
        new() { Status = status, Result = result };

    public static OperationResult<T> FromException(Exception exception) => 
        new() { Status = 500, Exception = exception, Errors = [exception.Message] };
    public static OperationResult<T> FromException(int status, Exception exception) => 
        new() { Status = status, Exception = exception, Errors = [exception.Message] };

    public static OperationResult<T> FromErrors(IEnumerable<string> errors) =>
        new OperationResult<T> { Status = 500, Errors = errors };
    public static OperationResult<T> FromErrors(int status, IEnumerable<string> errors) =>
        new OperationResult<T> { Status = status, Errors = errors };
}
