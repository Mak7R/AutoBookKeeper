using System.Security.Claims;
using System.Text;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Services;
using AutoBookKeeper.Core.Configuration;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories;
using AutoBookKeeper.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AutoBookKeeper.Web.Extensions;

public static class ConfigureServicesExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions(configuration);
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

        services.AddAutoMapper(typeof(Program).Assembly);
        
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(postgresConnectionString, b => b.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auto Book Keeper Web API V1", Version = "1.0" });
        });
        
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddHttpClient();

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => options.SetupDefaultConfig(configuration));
        
        services.AddApiVersioning(config =>
        {
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
        });
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>() ??
                                    throw new InvalidOperationException("AllowedOrigins was not found"));
                builder.WithHeaders(configuration.GetSection("AllowedHeaders").Get<string[]>() ??
                                    throw new InvalidOperationException("AllowedHeaders was not found"));
                builder.WithMethods(configuration.GetSection("AllowedMethods").Get<string[]>() ??
                                    throw new InvalidOperationException("AllowedMethods was not found"));
            });
        });
    }
    
    private static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IBooksRepository, BooksRepository>();
        services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        services.AddScoped<ITransactionTypesRepository, TransactionTypesRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();
        
        services.AddScoped<ICalculationsProvider, CalculationsProvider>();
        services.AddScoped<IForecastProvider, ForecastProvider>();

    }
    
    private static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddTransient<IAuthenticationService, JwtAuthenticationService>();
        
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IBooksService, BooksService>();
        services.AddScoped<ITransactionsService, TransactionsService>();
        services.AddScoped<ITransactionTypesService, TransactionTypesService>();
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IUserTokensRepository, UserTokensRepository>();

        services.AddScoped<ICalculationsService, CalculationsService>();
        services.AddScoped<IForecastService, ForecastService>();
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtAuthenticationOptions>(configuration.GetSection("JwtAuthentication"));
    }
}