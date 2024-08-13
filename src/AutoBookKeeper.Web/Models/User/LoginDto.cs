using System.ComponentModel.DataAnnotations;
using static AutoBookKeeper.Core.Rules.Length.UserStrings;

namespace AutoBookKeeper.Web.Models.User;

public class LoginDto
{
    [Required]
    [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
    public string Password { get; set; } = string.Empty;
}