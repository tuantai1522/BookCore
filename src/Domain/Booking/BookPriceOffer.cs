using Domain.Authoring;
using Shared;

namespace Domain.Booking;

public sealed class BookPriceOffer
{
    public Guid BookId { get; private set; }

    public Guid PriceOfferId { get; private set; }

    private BookPriceOffer(Guid bookId, Guid priceOfferId)
    {
        BookId = bookId;
        PriceOfferId = priceOfferId;
    }

    internal static Result<BookPriceOffer> Create(
        Guid bookId,
        Guid priceOfferId)
    {
        return Result.Success(new BookPriceOffer(bookId, priceOfferId));
    }
}
