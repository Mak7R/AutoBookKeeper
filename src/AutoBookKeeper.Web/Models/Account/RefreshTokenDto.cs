using System.ComponentModel.DataAnnotations;

namespace AutoBookKeeper.Web.Models.Account;

public class RefreshTokenDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}