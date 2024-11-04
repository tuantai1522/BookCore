using BookCore.Domain.Booking;
using BookCore.Shared;
using Shared;

namespace BookCore.Domain.Ordering;

public sealed class Order(string customerName, Address address)
    : AggregateRoot<Order, Guid>
{
    public string CustomerName { get; private set; } = customerName;

    public Address Address { get; private set; } = address;

    //-----------------------------------------------
    //relationships
    public IReadOnlyCollection<LineItem> items => [.. _items];

    private ICollection<LineItem> _items = [];

    public void AddLineItem(Book book, decimal finalPrice, int quantity)
    {
        var lineItem = LineItem.Create(book.Id, Id, finalPrice, quantity);
        _items.Add(lineItem.Value);
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
        return Result.Success(order);
    }
}
