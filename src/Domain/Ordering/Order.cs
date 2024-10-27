using Domain.Booking;
using Shared;

namespace Domain.Ordering;

public sealed class Order
    : Entity, IAggregateRoot
{
    public string CustomerName { get; private set; } = default!;

    public Address Address { get; private set; } = default!;

    //-----------------------------------------------
    //relationships
    public IReadOnlyCollection<LineItem> items => [.. _items];

    private ICollection<LineItem> _items = [];

    private Order(string customerName, Address address)
    {
        this.CustomerName = customerName;
        this.Address = address;
    }

    public void AddLineItems(IEnumerable<LineItem> items)
    {
        foreach (var item in items)
        {
            this._items.Add(item);
        }
    }

    public static Result<Order> Create(
        string customerName,
        Address address
        )
    {
        // Validate
        if (string.IsNullOrEmpty(address.Value))
        {
            return Result.Failure<Order>(Error.BadRequest("Address can not be null or empty"));
        }
        if (string.IsNullOrEmpty(address.City))
        {
            return Result.Failure<Order>(Error.BadRequest("City can not be null or empty"));
        }
        if (string.IsNullOrEmpty(address.ZipCode))
        {
            return Result.Failure<Order>(Error.BadRequest("Zip code can not be null or empty"));
        }

        Order order = new(customerName, address);
        return Result.Success<Order>(order);
    }
}
