using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.UserRules;

namespace AutoBookKeeper.Web.Models.User;

public class UpdateUserDto
{
    [Required]
    [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength)]
    public string UserName { get; set; } = string.Empty;
    
    [StringLength(MaxEmailLength)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(MaxPasswordLength)]
    public string CurrentPassword { get; set; } = string.Empty;
}