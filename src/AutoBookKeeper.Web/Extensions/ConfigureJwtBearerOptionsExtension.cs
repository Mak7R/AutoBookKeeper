using System.Security.Claims;
using System.Text;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Core.Configuration;
using AutoBookKeeper.Web.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AutoBookKeeper.Web.Extensions;

public static class ConfigureJwtBearerOptionsExtension
{
    public static void SetupDefaultConfig(this JwtBearerOptions options, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtAuthentication").Get<JwtAuthenticationOptions>();

        if (jwtOptions == null)
            throw new ConfigurationException("Required options 'JwtAuthentication' was not found");

        options.TokenValidationParameters = BuildTokenValidationParameters(jwtOptions);
        options.Events = BuildJwtBearerEvents();
    }

    private static TokenValidationParameters BuildTokenValidationParameters(JwtAuthenticationOptions jwtOptions)
    {
        if (string.IsNullOrEmpty(jwtOptions.SecretKey) || jwtOptions.SecretKey.Length < 32)
            throw new ConfigurationException("'JwtAuthentication:SecretKey' minimum length must be 32 characters");
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)
                )
        };

        tokenValidationParameters.ValidateAudience = !string.IsNullOrEmpty(jwtOptions.Audience);
        if (tokenValidationParameters.ValidateAudience)
            tokenValidationParameters.ValidAudience = jwtOptions.Audience;

        tokenValidationParameters.ValidateIssuer = !string.IsNullOrEmpty(jwtOptions.Issuer);
        if (tokenValidationParameters.ValidateIssuer)
            tokenValidationParameters.ValidIssuer = jwtOptions.Issuer;

        return tokenValidationParameters;
    }

    private static JwtBearerEvents BuildJwtBearerEvents()
    {
        return new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userIdString = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdString, out var userId))
                {
                    context.Fail("Unauthorized");
                    return;
                }
                
                var userService = context.HttpContext.RequestServices.GetRequiredService<IUsersService>();

                var user = await userService.GetByIdAsync(userId);

                if (user == null)
                {
                    context.Fail("Unauthorized");
                    return;
                }
            }
        };
    }
}