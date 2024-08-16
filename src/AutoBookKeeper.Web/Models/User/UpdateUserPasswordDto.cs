using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.UserRules;

namespace AutoBookKeeper.Web.Models.User;

public class UpdateUserPasswordDto
{
    [Required]
    [StringLength(MaxPasswordLength)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength)]
    public string NewPassword { get; set; } = string.Empty;
}