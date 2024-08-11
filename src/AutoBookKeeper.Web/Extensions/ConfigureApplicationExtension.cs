using AutoBookKeeper.Infrastructure.Data;

namespace AutoBookKeeper.Web.Extensions;

public static class ConfigureApplicationExtension
{
    public static void ConfigureApplication(this IHost app, IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        SeedDataBase(services, loggerFactory, configuration);
    }

    private static void SeedDataBase(IServiceProvider services, ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        try
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            ApplicationDbContextSeed.SeedAsync(dbContext, configuration, loggerFactory).Wait();
        }
        catch (Exception exception)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(exception, "An error occurred seeding the database.");
        }
    }
}