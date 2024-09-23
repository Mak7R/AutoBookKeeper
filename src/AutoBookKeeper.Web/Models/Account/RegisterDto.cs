using System.ComponentModel.DataAnnotations;
using AutoBookKeeper.Web.Validators;

using static AutoBookKeeper.Core.Rules.UserRules;

namespace AutoBookKeeper.Web.Models.Account;

public class RegisterDto
{
    [Required]
    [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength)]
    [AllowedCharacters(UserNameAllowedCharacters)]
    public string UserName { get; set; } = string.Empty;
    
    [StringLength(MaxEmailLength)]
    [EmailAddress] // todo should not throw error when is empty
    public string? Email { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
    [AllowedCharacters(PasswordAllowedCharacters)]
    public string Password { get; set; } = string.Empty;
}