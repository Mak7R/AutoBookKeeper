using System.ComponentModel.DataAnnotations;

namespace AutoBookKeeper.Core.Entities.Base;

public class Entity<TId> : IEntity<TId>
{
    [Key]
    public TId Id { get; protected set; }
}