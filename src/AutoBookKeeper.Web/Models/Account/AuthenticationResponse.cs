using AutoBookKeeper.Web.Models.User;

namespace AutoBookKeeper.Web.Models.Account;


public class AuthenticationResponse
{
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}