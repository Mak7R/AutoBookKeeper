using System.ComponentModel.DataAnnotations;

namespace AutoBookKeeper.Web.Models.User;

using static AutoBookKeeper.Core.Rules.Length.UserStrings;

public class UpdateUserPasswordDto
{
    [Required]
    [StringLength(MaxPasswordLength)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength)]
    public string NewPassword { get; set; } = string.Empty;
}