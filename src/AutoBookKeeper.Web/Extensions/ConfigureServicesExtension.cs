using AutoBookKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoBookKeeper.Web.Extensions;

public static class ConfigureServicesExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDefaultServices(configuration);
        services.ConfigureApplicationServices(configuration);
        services.ConfigureInfrastructureServices(configuration);
        
        return services;
    }

    private static void ConfigureDefaultServices(this IServiceCollection services, IConfiguration configuration)
    {
        var activeConnection = configuration["ActiveConnection"] ?? "DefaultConnection";
        var postgresConnectionString = configuration.GetConnectionString(activeConnection) ??
                                        throw new InvalidOperationException(
                                            $"Connection string '{activeConnection}' not found.");
        
        services.AddControllers();
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(postgresConnectionString, b => b.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    private static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        
    }
    
    private static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        
    }
}