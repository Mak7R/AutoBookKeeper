using AutoBookKeeper.Application.Models.Base;

namespace AutoBookKeeper.Application.Models;

public class TransactionModel : BaseModel<Guid>
{
    public string NameIdentifier { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime TransactionTime { get; set; }
    
    public Guid TypeId { get; set; }
    public TransactionTypeModel Type { get; set; }
    
    public Guid BookId { get; set; }
}