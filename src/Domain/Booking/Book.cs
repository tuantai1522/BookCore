using Domain.Authoring;
using Shared;

namespace Domain.Booking;

public sealed class Book :
    Entity, IAggregateRoot
{
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public DateTime PublishedOn { get; private set; }
    public decimal Price { get; private set; }

    //-----------------------------------------------
    //relationships

    public IReadOnlyCollection<BookAuthor> Authors => [.. _authors];

    private ICollection<BookAuthor> _authors = [];

    public IReadOnlyCollection<BookPriceOffer> PriceOffers => [.. _priceOffers];

    private ICollection<BookPriceOffer> _priceOffers = [];

    public IReadOnlyCollection<Review> Reviews => [.. _reviews];

    private ICollection<Review> _reviews = [];

    //contructors
    private Book(string title, string description, DateTime publishedOn, decimal price)
    {
        Title = title;
        Description = description;
        PublishedOn = publishedOn;
        Price = price;
    }

    //methods
    public static Result<Book> Create(
        string title,
        string description,
        DateTime publishedOn,
        decimal price
        )
    {
        // Validate
        if (price < 0)
        {
            return Result.Failure<Book>(Error.BadRequest("Price must be greater than 0"));
        }
        Book book = new(title, description, publishedOn, price);

        return Result.Success(book);
    }

    public void AddAuthor(Author author)
    {
        Result<BookAuthor> bookAuthor = BookAuthor.Create(Id, author.Id);

        _authors.Add(bookAuthor.Value);
    }

    public void AddPriceOffer(PriceOffer priceOffer)
    {
        Result<BookPriceOffer> bookPriceOffer = BookPriceOffer.Create(Id, priceOffer.Id);

        _priceOffers.Add(bookPriceOffer.Value);
    }

    public void AddReview(Review review)
    {
        _reviews.Add(review);
    }
}
