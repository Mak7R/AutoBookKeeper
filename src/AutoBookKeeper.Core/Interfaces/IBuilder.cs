namespace AutoBookKeeper.Core.Interfaces;

public interface IBuilder<out T>
{
    T Build();
}