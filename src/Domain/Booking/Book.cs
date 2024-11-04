using BookCore.Domain.Authoring;
using BookCore.Shared;
using Shared;

namespace BookCore.Domain.Booking;

public sealed class Book(string title, string description, DateTime publishedOn, decimal price)
    : AggregateRoot<Book, Guid>
{
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public DateTime PublishedOn { get; private set; } = publishedOn;
    public decimal Price { get; private set; } = price;

    //-----------------------------------------------
    //relationships

    public IReadOnlyCollection<Author> Authors => [.. _authors];

    private ICollection<Author> _authors = [];

    public IReadOnlyCollection<BookPriceOffer> BookPriceOffers => [.. _bookPriceOffers];

    private ICollection<BookPriceOffer> _bookPriceOffers = [];

    public IReadOnlyCollection<Review> Reviews => [.. _reviews];

    private ICollection<Review> _reviews = [];

    //methods
    public static Result<Book> Create(string title, string description, DateTime publishedOn, decimal price)
    {
        // Validate
        if (price < 0)
        {
            return Result.Failure<Book>(Error.BadRequest("Price must be greater than 0"));
        }
        Book book = new(title, description, publishedOn, price);

        return Result.Success(book);
    }

    public void AddPriceOffer(PriceOffer priceOffer, DateTime startDate, DateTime endDate)
    {
        Result<BookPriceOffer> bookPriceOffer = BookPriceOffer.Create(Id, priceOffer.Id, startDate, endDate);

        _bookPriceOffers.Add(bookPriceOffer.Value);
    }

    public void AddReview(Review review)
    {
        _reviews.Add(review);
    }
}
