namespace AutoBookKeeper.Application.Exceptions;

public class ApplicationException : Exception
{
    internal ApplicationException()
    {
    }

    internal ApplicationException(string message) : base(message)
    {
    }

    internal ApplicationException(string message, Exception inner) : base(message, inner)
    {
    }
}