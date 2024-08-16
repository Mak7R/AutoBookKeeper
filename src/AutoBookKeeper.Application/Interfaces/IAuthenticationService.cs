using AutoBookKeeper.Application.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IAuthenticationService
{
    Task<(string AccessToken, string RefreshToken)?> GenerateTokenAsync(UserModel user);

    Task<(string AccessToken, string RefreshToken)?> RefreshAccessTokenAsync(UserModel user, string refreshToken);
}
