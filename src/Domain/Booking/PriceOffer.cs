using BookCore.Shared;
using Shared;

namespace BookCore.Domain.Booking;

public sealed class PriceOffer(double DiscountPercentage, string promotionalText)
    : AggregateRoot<PriceOffer, Guid>
{
    public double DiscountPercentage { get; private set; } = DiscountPercentage;

    public string PromotionalText { get; private set; } = promotionalText;

    public IReadOnlyCollection<BookPriceOffer> BookPriceOffers => [.. _bookPriceOffers];

    private ICollection<BookPriceOffer> _bookPriceOffers = [];


    public static Result<PriceOffer> Create(double discountPercentage, string promotionalText)
    {
        return Result.Success(new PriceOffer(discountPercentage, promotionalText));
    }

    public void AddBook(Book book, DateTime startDate, DateTime endDate)
    {
        Result<BookPriceOffer> bookPriceOffer = BookPriceOffer.Create(book.Id, Id, startDate, endDate);

        _bookPriceOffers.Add(bookPriceOffer.Value);
    }
}
