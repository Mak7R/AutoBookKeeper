namespace AutoBookKeeper.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    internal NotFoundException()
    {
    }

    internal NotFoundException(string message) : base(message)
    {
    }

    internal NotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}