using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace BookCore.Application.Queries.CriteriaQueries;

public static class MapToPaginationResponse
{
    public static IQueryable<TResponse> Handle<TEntity, TResponse>(this IQueryable<TEntity> query, IMapper mapper)
    {
        var mappedQuery = query.ProjectTo<TResponse>(mapper.ConfigurationProvider);

        return mappedQuery;
    }
}
