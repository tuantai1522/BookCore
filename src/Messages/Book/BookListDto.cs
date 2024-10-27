namespace BookCore.Messages.Book;

public sealed record BookListDto
{
    public int BookId { get; }
    public string Title { get; } = default!;
    public DateTime PublishedOn { get; }
    public decimal Price { get; }
    public decimal ActualPrice { get; }
    public string? PromotionPromotionalText { get; }
    public string AuthorsOrdered { get; } = default!;

    public int ReviewsCount { get; }
    public double? ReviewsAverageVotes { get; }
}
