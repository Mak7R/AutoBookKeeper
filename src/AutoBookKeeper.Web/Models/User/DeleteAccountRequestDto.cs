using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.UserRules;

namespace AutoBookKeeper.Web.Models.User;

public class DeleteAccountRequestDto
{
    [Required]
    [StringLength(MaxPasswordLength)]
    public string Password { get; set; } = string.Empty;
}