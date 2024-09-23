using AutoBookKeeper.Core.Entities.Base;

namespace AutoBookKeeper.Core.Entities;

public class Book : Entity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public DateTime CreationTime { get; set; }
}
