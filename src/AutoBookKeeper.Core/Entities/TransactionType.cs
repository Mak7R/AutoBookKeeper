using AutoBookKeeper.Core.Entities.Base;

namespace AutoBookKeeper.Core.Entities;

public class TransactionType : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}