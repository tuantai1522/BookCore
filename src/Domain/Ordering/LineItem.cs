using Domain.Authoring;
using Domain.Booking;
using Shared;

namespace Domain.Ordering;

public sealed class LineItem
    : Entity
{
    public Guid OrderId { get; private set; }
    public Guid BookId { get; private set; }
    public Book Book { get; private set; } = default!;

    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    private LineItem(Guid bookId, Guid orderId, decimal unitPrice, int quantity)
    {
        this.BookId = bookId;
        this.OrderId = orderId;
        this.UnitPrice = unitPrice;
        this.Quantity = quantity;
    }
    public static Result<LineItem> Create(
        Guid bookId, 
        Guid orderId, 
        decimal unitPrice, 
        int quantity
    )
    {
        if (unitPrice < 0)
        {
            return Result.Failure<LineItem>(Error.BadRequest("Unit price must be greater than 0"));
        }
        if (quantity < 0)
        {
            return Result.Failure<LineItem>(Error.BadRequest("Quantity must be greater than 0"));
        }

        LineItem item = new(bookId, orderId, unitPrice, quantity);

        return Result.Success(item);
    }
}
