using AutoBookKeeper.Application.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IAuthenticationService
{
    string GenerateToken(UserModel user);
}
