namespace AutoBookKeeper.Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    internal InfrastructureException()
    {
    }

    internal InfrastructureException(string message) : base(message)
    {
    }

    internal InfrastructureException(string message, Exception inner) : base(message, inner)
    {
    }
}