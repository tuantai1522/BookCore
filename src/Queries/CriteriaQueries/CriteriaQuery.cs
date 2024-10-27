using AutoMapper;
using BookCore.Messages.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BookCore.Application.Queries.CriteriaQueries;

public static class CriteriaQuery
{
    /// <summary>
    /// Handles the process of applying filtering, sorting, and pagination to a queryable data source and returns a paginated response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <typeparam name="TResponse">The type of the response object that the entity will be mapped to.</typeparam>
    /// <param name="query">The initial queryable data source to be processed.</param>
    /// <param name="response">The pagination request containing filter, sort, and pagination information.</param>
    /// <param name="mapper">The AutoMapper instance used to map entities to response objects.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the paginated response with the filtered and sorted data.</returns>
    public static async Task<PaginationResponse<TResponse>> Handle<TEntity, TResponse>(
        this IQueryable<TEntity> query,
        PaginationRequest<TResponse> request,
        IMapper mapper,
        CancellationToken cancellationToken = default) where TResponse : class
    {
        // Map the query to a pagination response with mapping applied.
        var queryResponse = query.Handle<TEntity, TResponse>(mapper);

        // Translate string to ExpressionTree
        var expressionTree = TranslateToExpressionTree.Translate(request.Filters);

        // Apply filtering to the query response based on the provided filters.
        queryResponse = QueryFilteringHandler.Handle(queryResponse, expressionTree);

        // Apply sorting to the query response based on the provided sort criteria.
        queryResponse = queryResponse.Handle(request.Sort);

        // Get the total count of items in the query response asynchronously.
        var rowCount = await queryResponse.CountAsync(cancellationToken);

        // Apply pagination to the query response, considering the total row count, page number, and page size.
        var result = await PaginationHandler.Handle(queryResponse, rowCount, request.Page, request.PageSize, cancellationToken);

        // Return the paginated result.
        return result;
    }
}
