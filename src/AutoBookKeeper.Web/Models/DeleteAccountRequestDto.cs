using System.ComponentModel.DataAnnotations;

using static AutoBookKeeper.Core.Rules.Length.UserStrings;

namespace AutoBookKeeper.Web.Models;

public class DeleteAccountRequestDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(MaxPasswordLength)]
    public string Password { get; set; } = string.Empty;
}