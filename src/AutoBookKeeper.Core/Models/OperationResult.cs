
namespace AutoBookKeeper.Core.Models;

public class OperationResult : OperationResult<object>;

public class OperationResult<T>
{
    public bool IsSuccessful => Status is >= 200 and < 300;
    public int Status { get; set; }
    public T? Result { get; set; } = default;
    public Exception? Exception { get; set; } = null;
    public IEnumerable<string> Errors { get; set; } = [];

    public static OperationResult<T> FromStatus(int status) => 
        new OperationResult<T> { Status = status };

    public static OperationResult<T> FromResult(T? result) => 
        new OperationResult<T> { Status = 200, Result = result };
    public static OperationResult<T> FromResult(int status, T? result) => 
        new OperationResult<T> { Status = status, Result = result };

    public static OperationResult<T> FromException(Exception exception) => 
        new OperationResult<T> { Status = 500, Exception = exception, Errors = [exception.Message] };
    public static OperationResult<T> FromException(int status, Exception exception) => 
        new OperationResult<T> { Status = status, Exception = exception, Errors = [exception.Message] };

    public static OperationResult<T> FromErrors(IEnumerable<string> errors) =>
        new OperationResult<T> { Status = 500, Errors = errors };
    public static OperationResult<T> FromErrors(int status, IEnumerable<string> errors) =>
        new OperationResult<T> { Status = status, Errors = errors };
}
