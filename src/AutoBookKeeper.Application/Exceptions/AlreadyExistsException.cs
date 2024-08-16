namespace AutoBookKeeper.Application.Exceptions;

public class AlreadyExistsException : ApplicationException
{
    internal AlreadyExistsException()
    {
    }

    internal AlreadyExistsException(string message) : base(message)
    {
    }

    internal AlreadyExistsException(string message, Exception inner) : base(message, inner)
    {
    }
}