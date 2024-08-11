using System.ComponentModel.DataAnnotations;
using AutoBookKeeper.Core.Entities.Base;

using static AutoBookKeeper.Core.Rules.Length.UserStrings;

namespace AutoBookKeeper.Core.Entities;

public class User : Entity<Guid>
{
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(MaxEmailLength)]
    public string? Email { get; set; }
    
    [StringLength(MaxPasswordHashLength)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public bool IsEmailConfirmed { get; set; }
}
