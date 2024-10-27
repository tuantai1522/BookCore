using System.Linq.Expressions;

namespace BookCore.Application.Queries.CriteriaQueries;

public static class SortHandler
{
    /// <summary>
    /// Sorts an IQueryable collection based on a specified sort string.
    /// </summary>
    /// <typeparam name="TResponse">The type of the elements in the IQueryable collection.</typeparam>
    /// <param name="query">The IQueryable collection to be sorted.</param>
    /// <param name="sortString">
    /// A string specifying the property to sort by and the sorting order.
    /// The format should be "PropertyName=asc" or "PropertyName=desc". 
    /// If the sorting order is not specified, it defaults to ascending.
    /// </param>
    /// <returns>A sorted IQueryable collection based on the specified sorting criteria.</returns>
    public static IQueryable<TResponse> Handle<TResponse>(this IQueryable<TResponse> query, string? sortString)
    {
        // check null
        if (sortString == null)
        {
            return query;
        }

        // Split the sort string into property name and sort direction
        var sortParts = sortString.Split('=');
        var propertyName = sortParts[0].Trim();
        var sortValue = sortParts.Length > 1 ? sortParts[1].Trim() : "asc";

        // Get the property info for the given property name
        var propertyInfo = typeof(TResponse).GetProperty(propertyName);

        // Check if the property has the CriteriaQueryAttribute and if sorting is allowed
        var attributes = propertyInfo?.GetCustomAttributes(typeof(CriteriaQueryAttribute), true)
                                       .Cast<CriteriaQueryAttribute>()
                                       .FirstOrDefault();

        if (attributes != null && attributes.NoSort)
        {
            // Return the original query if the attribute is not present or sorting is not allowed
            return query;
        }

        if (propertyInfo == null)
        {
            // Return the original query if the property does not exist
            return query;
        }

        // Create a parameter expression representing a parameter of type TResponse (used in lambda)
        var parameter = Expression.Parameter(typeof(TResponse), "x");

        // Create an expression to access the property on the parameter
        var property = Expression.Property(parameter, propertyInfo);

        // Create a lambda expression for the property access
        var lambda = Expression.Lambda(property, parameter);

        // Determine the method name based on the sort direction (asc or desc)
        var methodName = sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

        // Get the appropriate OrderBy or OrderByDescending method for IQueryable<T>
        var method = typeof(Queryable).GetMethods()
                                      .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                                      .MakeGenericMethod(typeof(TResponse), propertyInfo.PropertyType);

        // Invoke the method to apply sorting to the query
        var result = method.Invoke(null, new object[] { query, lambda });

        // Return the sorted query or the original query if sorting failed
        return result as IQueryable<TResponse> ?? query;
    }
}
