using BookCore.Messages.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BookCore.Application.Queries.CriteriaQueries;

public static class PaginationHandler
{
    public static async Task<PaginationResponse<TResponse>> Handle<TResponse>(
        this IQueryable<TResponse> query, int rowCount,
        int? page, int? pageSize,
        CancellationToken cancellationToken = default) where TResponse : class
    {
        // Default to 1 if page is null or less than 1
        int activePage = page.HasValue && page.Value > 0 ? page.Value : 1;

        // Default to 10 if pageSize is null or less than 1
        int pageSizeNumber = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 10;

        int skip = (activePage - 1) * pageSizeNumber;

        var items = await query.Skip(skip).Take(pageSizeNumber).ToListAsync(cancellationToken);

        return new PaginationResponse<TResponse>(items, rowCount, activePage, pageSizeNumber);
    }
}
