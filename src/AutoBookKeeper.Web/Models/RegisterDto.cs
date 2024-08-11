using System.ComponentModel.DataAnnotations;
using static AutoBookKeeper.Core.Rules.Length.UserStrings;

namespace AutoBookKeeper.Web.Models;

public class RegisterDto
{
    [Required]
    [StringLength(MaxNameLength, MinimumLength = MinNameLength, ErrorMessage = "Invalid name length")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(MaxEmailLength)]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength, ErrorMessage = "Invalid password length")]
    public string Password { get; set; } = string.Empty;
}