namespace AutoBookKeeper.Core.Entities.Base;

public interface IEntity<out TId>
{
    TId Id { get; }
}