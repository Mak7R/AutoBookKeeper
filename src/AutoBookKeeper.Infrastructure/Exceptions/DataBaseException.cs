namespace AutoBookKeeper.Infrastructure.Exceptions;

public class DataBaseException : InfrastructureException
{
    internal DataBaseException()
    {
    }

    internal DataBaseException(string message) : base(message)
    {
    }

    internal DataBaseException(string message, Exception inner) : base(message, inner)
    {
    }
}