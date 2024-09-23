using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.UserRules;

namespace AutoBookKeeper.Web.Models.Account;

public class LoginDto
{
    [Required]
    [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
    public string Password { get; set; } = string.Empty;
}