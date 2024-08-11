namespace AutoBookKeeper.Core.Configuration;

public class JwtAuthenticationOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpirationMinutes { get; set; }
    public string SecretKey { get; set; } = string.Empty;
}