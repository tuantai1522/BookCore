using Shared;

namespace BookCore.Domain.Booking;

public sealed class BookPriceOffer(Guid bookId, Guid priceOfferId, DateTime startDate, DateTime endDate)
{
    public Guid BookId { get; private set; } = bookId;

    public Guid PriceOfferId { get; private set; } = priceOfferId;

    public DateTime StartDate { get; private set; } = startDate;
    public DateTime EndDate { get; private set; } = endDate;

    public static Result<BookPriceOffer> Create(Guid bookId, Guid priceOfferId, DateTime startDate, DateTime endDate)
    {
        return Result.Success(new BookPriceOffer(bookId, priceOfferId, startDate, endDate));
    }

}
