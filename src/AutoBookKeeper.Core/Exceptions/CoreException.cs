namespace AutoBookKeeper.Core.Exceptions;

public class CoreException : Exception
{
    internal CoreException()
    {
    }

    internal CoreException(string message) : base(message)
    {
    }

    internal CoreException(string message, Exception inner) : base(message, inner)
    {
    }
}