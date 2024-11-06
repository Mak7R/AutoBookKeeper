using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Helpers;

public static class ApplicationExceptionsHandlingHelper
{
    private const string RetrieveDataExceptionMessage = "An error occurred while retrieving data";
    
    public static Exception HandleRetrieveDataException(Exception exception, ILogger logger)
    {
        logger.LogError(exception, RetrieveDataExceptionMessage);
        return new ApplicationException(RetrieveDataExceptionMessage, exception);
    }

    public static Exception HandleMutateDataException(Exception exception, string errorMessage, ILogger logger)
    {
        logger.LogError(exception, errorMessage);
        return new ApplicationException(errorMessage, exception);
    }
}