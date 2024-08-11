using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoBookKeeper.Infrastructure.Data;

public class ApplicationDbContextSeed
{
    public static int MaxRetryCount { get; set; } = 3;
    public static async Task SeedAsync(ApplicationDbContext dbContext, IConfiguration configuration, ILoggerFactory loggerFactory, int retry = 0)
    { 
        try
        {
            //await dbContext.Database.EnsureCreatedAsync();
            if (dbContext.Database.IsRelational()) 
                await dbContext.Database.MigrateAsync();

            await SeedUsersAsync(dbContext);
        }
        catch (Exception exception)
        {
            var logger = loggerFactory.CreateLogger<ILogger<ApplicationDbContextSeed>>();
            logger.LogError(exception, "Exception was occured while seeding db context");

            if (retry < MaxRetryCount)
                await SeedAsync(dbContext, configuration, loggerFactory, ++retry);
            else
                throw;
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext dbContext)
    {
        // seed users
    }
}