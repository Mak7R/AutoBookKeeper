namespace AutoBookKeeper.Web.Models.User;

public class AuthenticationResponse
{
    public UserProfileViewModel User { get; set; }
    public string Token { get; set; } = string.Empty;
}