namespace AutoBookKeeper.Web.Exceptions;

public class ConfigurationException : WebException
{
    internal ConfigurationException()
    {
    }

    internal ConfigurationException(string message) : base(message)
    {
    }

    internal ConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }
}