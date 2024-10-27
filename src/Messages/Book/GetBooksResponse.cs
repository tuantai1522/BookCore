namespace BookCore.Messages.Book;

public class GetBooksResponse
{
    public int ActivePage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public IReadOnlyList<BookListDto> Items { get; set; } = default!;
}
