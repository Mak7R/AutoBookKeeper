namespace AutoBookKeeper.Web.Exceptions;

public class WebException : Exception
{
    internal WebException()
    {
    }

    internal WebException(string message) : base(message)
    {
    }

    internal WebException(string message, Exception inner) : base(message, inner)
    {
    }
}