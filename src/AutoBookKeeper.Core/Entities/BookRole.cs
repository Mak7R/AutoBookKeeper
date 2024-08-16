using AutoBookKeeper.Core.Entities.Base;
using AutoBookKeeper.Core.ValueObjects;

namespace AutoBookKeeper.Core.Entities;

public class BookRole : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public BookAccess Access { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
