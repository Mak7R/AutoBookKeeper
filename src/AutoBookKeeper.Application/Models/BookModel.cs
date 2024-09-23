using AutoBookKeeper.Application.Models.Base;

namespace AutoBookKeeper.Application.Models;

public class BookModel : BaseModel<Guid>
{
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public UserModel Owner { get; set; }
    
    public DateTime CreationTime { get; set; }
}