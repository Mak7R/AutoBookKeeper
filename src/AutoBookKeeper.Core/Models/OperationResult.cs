

namespace AutoBookKeeper.Core.Models;

public class OperationResult(int status) : OperationResult<object>(status);

public class OperationResult<T>
{
    public bool IsSuccessful => Status is >= 200 and < 300;
    public int Status { get; init; }
    public T? Result { get; init; }
    public string? ErrorMessage { get; init; }
    public IDictionary<string, string[]> ValidationErrors { get; init; } = new Dictionary<string, string[]>();

    public OperationResult(int status)
    {
        Status = status;
    }
}