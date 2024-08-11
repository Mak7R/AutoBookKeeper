namespace AutoBookKeeper.Web.Models;

public class UserProfileViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
}