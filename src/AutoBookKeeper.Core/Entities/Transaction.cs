using AutoBookKeeper.Core.Entities.Base;

namespace AutoBookKeeper.Core.Entities;

public class Transaction : Entity<Guid>
{
    public Guid BookId { get; set; }
    public Book Book { get; set; }
    public string NameIdentifier { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionTime { get; set; }
}