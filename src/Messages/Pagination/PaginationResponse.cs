namespace BookCore.Messages.Pagination;

public class PaginationResponse<TResponse>(
    IEnumerable<TResponse> items, 
    int count, 
    int activePage, 
    int pageSize) where TResponse : class
{
    public int ActivePage { get; set; } = activePage;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalItems { get; set; } = count;
    public IList<TResponse> Items { get; set; } = new List<TResponse>(items);
}
