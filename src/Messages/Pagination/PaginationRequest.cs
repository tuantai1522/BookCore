using MediatR;
using Shared;

namespace BookCore.Messages.Pagination;

public abstract class PaginationRequest<TResponse> 
    : IRequest<Result<PaginationResponse<TResponse>>> where TResponse : class
{
    public string? Filters { get; set; }
    public string? Sort { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}
