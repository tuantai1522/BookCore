using Shared;

namespace Domain.Booking;

public sealed class PriceOffer :
    Entity, IAggregateRoot

{
    public double DiscountPercentage { get; private set; }

    public string PromotionalText { get; private set; } = default!;

    private PriceOffer(double DiscountPercentage, string promotionalText)
    {
        this.DiscountPercentage = DiscountPercentage;
        this.PromotionalText = promotionalText;
    }

    public IReadOnlyCollection<BookPriceOffer> BookPriceOffers => [.. _bookPriceOffers];

    private ICollection<BookPriceOffer> _bookPriceOffers = [];

    internal static Result<PriceOffer> Create(
        double discountPercentage, 
        string promotionalText)
    {
        return Result.Success(new PriceOffer(discountPercentage, promotionalText));
    }
}
