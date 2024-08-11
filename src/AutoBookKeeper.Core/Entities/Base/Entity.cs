namespace AutoBookKeeper.Core.Entities.Base;

public class Entity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; }
    
}