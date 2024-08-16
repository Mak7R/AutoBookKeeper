using AutoBookKeeper.Core.Entities.Base;

namespace AutoBookKeeper.Core.Entities;

public class User : Entity<Guid>
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    
    public bool IsEmailConfirmed { get; set; }
}
