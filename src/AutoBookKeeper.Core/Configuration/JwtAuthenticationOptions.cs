namespace AutoBookKeeper.Core.Configuration;

public class JwtAuthenticationOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpirationMinutes { get; set; }
    public string SecretKey { get; set; } = string.Empty;
    
    public int RefreshTokenExpirationMinutes { get; set; }
    public string RefreshTokenChoices { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public int RefreshTokenLength { get; set; } = 32;
}