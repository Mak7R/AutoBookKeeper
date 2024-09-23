namespace AutoBookKeeper.Core.Rules;

public static class UserRules
{
    public const int MinUserNameLength = 4;
    public const int MaxUserNameLength = 64;
    public const int MaxEmailLength = 320;
    public const int MaxPasswordHashLength = 128;
    public const int MaxPasswordLength = 256;
    public const int MinPasswordLength = 4;
    
    public const string UserNameAllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-!#$%";
    public const string PasswordAllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-!#$%";
}