using BookCore.Domain.Booking;
using Shared;

namespace BookCore.Domain.Ordering;

public sealed class LineItem
    : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid BookId { get; private set; }

    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    private LineItem(Guid bookId, Guid orderId, decimal unitPrice, int quantity)
    {
        BookId = bookId;
        OrderId = orderId;
        UnitPrice = unitPrice;
        Quantity = quantity;
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
