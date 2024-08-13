using AutoBookKeeper.Application.Models.Base;

namespace AutoBookKeeper.Application.Models;

public class UserModel : BaseModel<Guid>
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
}