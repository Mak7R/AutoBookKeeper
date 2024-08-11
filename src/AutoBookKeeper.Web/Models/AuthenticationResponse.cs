namespace AutoBookKeeper.Web.Models;

public class AuthenticationResponse
{
    public UserViewModel User { get; set; }
    public string Token { get; set; } = string.Empty;
}