using AutoBookKeeper.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Helpers;

public static class InfrastructureExceptionsHandlingHelper
{
    private const string DbUpdateErrorMessage = "Error was occured while saving to the database";
    public static Exception Handle(DbUpdateException dbUpdateException, ILogger logger, string operation)
    {
        logger.LogError(dbUpdateException, DbUpdateErrorMessage + " Operation: {operation}", operation);
        return new DataBaseException(DbUpdateErrorMessage, dbUpdateException);
    }

    private const string UnexpectedErrorMessage = "Unexpected error was occured";
    public static Exception Handle(Exception exception, ILogger logger)
    {
        logger.LogError(exception, UnexpectedErrorMessage);
        return new InfrastructureException(UnexpectedErrorMessage, exception);
    }
}