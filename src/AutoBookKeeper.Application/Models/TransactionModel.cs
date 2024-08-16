using AutoBookKeeper.Application.Models.Base;

namespace AutoBookKeeper.Application.Models;

public class TransactionModel : BaseModel<Guid>
{
    public BookModel Book { get; set; }
    public string NameIdentifier { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionTime { get; set; }
}