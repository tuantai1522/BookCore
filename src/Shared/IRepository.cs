using Shared;

namespace BookCore.Shared;
public interface IRepository<T> 
    where T : IAggregateRoot
{
}
