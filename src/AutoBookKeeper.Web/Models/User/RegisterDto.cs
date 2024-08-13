using System.ComponentModel.DataAnnotations;
using AutoBookKeeper.Core.Rules;
using AutoBookKeeper.Web.Validators;

using static AutoBookKeeper.Core.Rules.Length.UserStrings;

namespace AutoBookKeeper.Web.Models.User;

public class RegisterDto
{
    [Required]
    [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength, ErrorMessage = "Invalid name length")]
    [AllowedCharacters(AllowedCharacters.ForUser.UserName)]
    public string UserName { get; set; } = string.Empty;
    
    [StringLength(MaxEmailLength)]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength, ErrorMessage = "Invalid password length")]
    [AllowedCharacters(AllowedCharacters.ForUser.Password)]
    public string Password { get; set; } = string.Empty;
}