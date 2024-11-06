using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Configuration;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Specifications;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AutoBookKeeper.Application.Services;

public class JwtAuthenticationService : IAuthenticationService
{
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly JwtAuthenticationOptions _options;

    public JwtAuthenticationService(IUserTokensRepository userTokensRepository, IOptions<JwtAuthenticationOptions> options)
    {
        _userTokensRepository = userTokensRepository;
        _options = options.Value;
    }

    public async Task<(string AccessToken, string RefreshToken)?> GenerateTokenAsync(UserModel user)
    {
        var userTokens = await _userTokensRepository.GetAsync(UserTokenSpecification.TokensByUserId(user.Id));

        foreach (var userToken in userTokens)
        {
            if (false) // todo check device
                return (GenerateAccessToken(user), userToken.Token);
        }
        
        return (GenerateAccessToken(user), await GenerateRefreshTokenAsync(user));
    }
    
    public async Task<(string AccessToken, string RefreshToken)?> RefreshAccessTokenAsync(UserModel user, string refreshToken)
    {
        var userTokens = await _userTokensRepository.GetAsync(UserTokenSpecification.TokensByUserId(user.Id));

        foreach (var userToken in userTokens)
        {
            if (userToken.Token == refreshToken)
                return (GenerateAccessToken(user), userToken.Token);
        }
        
        return null;
    }
    
    private string GenerateAccessToken(UserModel user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        var key = _options.SecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.ExpirationMinutes));

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }

    private async Task<string> GenerateRefreshTokenAsync(UserModel user)
    {
        var refreshToken = GenerateRefreshToken();

        await _userTokensRepository.CreateAsync(new UserToken
        {
            UserId = user.Id, Token = refreshToken,
            ExpirationTime = DateTime.UtcNow.AddMinutes(_options.RefreshTokenExpirationMinutes)
        });

        return refreshToken;
    }

    private string GenerateRefreshToken()
    {
        return RandomNumberGenerator.GetString(_options.RefreshTokenChoices, _options.RefreshTokenLength);
    }
}