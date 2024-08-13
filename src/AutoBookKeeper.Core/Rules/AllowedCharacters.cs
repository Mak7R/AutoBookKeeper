
namespace AutoBookKeeper.Core.Rules;

public static class AllowedCharacters
{
    public static class ForUser
    {
        public const string UserName = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-!#$%";
        public const string Password = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-!#$%";
    }
}
