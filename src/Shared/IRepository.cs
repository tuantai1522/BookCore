namespace BookCore.Shared;
public interface IRepository<T, TId>
    where T : AggregateRoot<T, TId>
{
}