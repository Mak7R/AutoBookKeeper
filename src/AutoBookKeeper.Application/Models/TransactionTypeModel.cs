using AutoBookKeeper.Application.Models.Base;

namespace AutoBookKeeper.Application.Models;

public class TransactionTypeModel : BaseModel<Guid>
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public Guid BookId { get; set; }
}