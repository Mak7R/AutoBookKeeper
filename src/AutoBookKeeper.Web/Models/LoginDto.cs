using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.Length.UserStrings;

namespace AutoBookKeeper.Web.Models;

public class LoginDto
{
    [Required]
    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
    public string Password { get; set; } = string.Empty;
}