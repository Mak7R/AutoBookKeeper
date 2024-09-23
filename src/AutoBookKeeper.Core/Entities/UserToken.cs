using AutoBookKeeper.Core.Entities.Base;

namespace AutoBookKeeper.Core.Entities;

public class UserToken : Entity<int>
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpirationTime { get; set; }
}