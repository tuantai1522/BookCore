using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
namespace BookCore.Application.Queries.CriteriaQueries;

public static class QueryFilteringHandler
{
    /// <summary>
    /// Filters an IQueryable<TResponse> query based on the conditions provided in an ExpressionTree.
    /// </summary>
    /// <typeparam name="TResponse">The type of the entities in the IQueryable being queried.</typeparam>
    /// <param name="query">The original IQueryable to be filtered.</param>
    /// <param name="expressionTree">An ExpressionTree object that defines the filtering conditions and logical operators.</param>
    /// <returns>A filtered IQueryable<TResponse> that matches the conditions specified in the ExpressionTree.</returns>
    public static IQueryable<TResponse> Handle<TResponse>(IQueryable<TResponse> query, ExpressionTree expressionTree)
    {
        // If the expression tree is null or has no operands, return the original query.
        if (expressionTree == null || expressionTree.OperationExpressions == null || !expressionTree.OperationExpressions.Any())
            return query;

        // Get the parameter for the expression (like 'x' in 'x => x.Property')
        var parameter = Expression.Parameter(typeof(TResponse), "x");

        // Build the expression tree from the ExpressionTree object
        var expression = BuildExpressionTree<TResponse>(expressionTree, parameter);

        // If the expression is null, return the original query
        if (expression == null)
        {
            return query;
        }

        // Create a lambda expression (like 'x => x.Property == value')
        var lambda = Expression.Lambda<Func<TResponse, bool>>(expression, parameter);

        // Apply the lambda expression to the query using the Where clause
        return query.Where(lambda);
    }

    private static Expression? BuildExpressionTree<TResponse>(ExpressionTree expressionTree, ParameterExpression parameter)
    {
        Expression result = null!;

        if (expressionTree.OperationExpressions == null)
        {
            return result;
        }

        // Iterate over each operand in the expression tree
        foreach (var operation in expressionTree.OperationExpressions)
        {
            Expression? currentExpression = null;

            if (operation.SubExpression != null)
            {
                // Recursively build the expression for sub-expressions
                currentExpression = BuildExpressionTree<TResponse>(operation.SubExpression, parameter);
            }
            else if (!string.IsNullOrEmpty(operation.Condition))
            {
                // Build the expression for a single condition
                var match = Regex.Match(operation.Condition, Constant.ConditionPatternRegex);

                // Extract the property name, operator, and value from the matched groups
                var propertyName = match.Groups[1].Value.Trim();
                var operators = match.Groups[2].Value.Trim();
                var value = match.Groups[3].Value.Trim();

                // Handle if property is relations name
                Expression? propertyExpression = null;
                Type type = typeof(TResponse);
                bool relationsExist = true;
                if (propertyName.Contains('.'))
                {
                    int index = 0;
                    propertyExpression = parameter;
                    var propertypaths = propertyName.Split('.');
                    foreach (var property in propertypaths)
                    {
                        // check relation exist
                        if (type.GetProperty(property) == null)
                        {
                            relationsExist = false;
                            break;
                        }

                        propertyExpression = Expression.PropertyOrField(propertyExpression, property);
                        propertyName = property;
                        if (index < propertypaths.Length - 1)
                        {
                            type = propertyExpression.Type;
                        }
                        index++;
                    }
                }

                if (relationsExist)
                {
                    // Get the PropertyInfo for the property
                    var propertyInfo = type.GetProperty(propertyName);

                    // Proceed only if the property exists
                    if (propertyInfo != null)
                    {
                        // Get the property expression (like 'x.Property')
                        propertyExpression ??= Expression.Property(parameter, propertyName);

                        // Retrieve any custom attributes applied to the property
                        var attributes = propertyInfo?.GetCustomAttributes(typeof(CriteriaQueryAttribute), true)
                                                    .Cast<CriteriaQueryAttribute>()
                                                    .FirstOrDefault();

                        // Get the type of the property and convert the value to that type
                        var propertyType = propertyExpression.Type;
                        var typedValue = ConvertStringToType(value, propertyType);

                        if (typedValue != null)
                        {
                            var constantExpression = Expression.Constant(typedValue);

                            // If the property does not have the CanFilter attribute or can be filtered, proceed
                            if (attributes == null || !attributes.NoFilter)
                            {
                                // Validate if the property type is suitable for the comparison operators
                                if (IsValidForComparison(propertyInfo, operators))
                                {
                                    // Build the actual condition expression (like 'x.Property == value')
                                    currentExpression = BuildConditionExpression(propertyExpression, constantExpression, operators);
                                }
                            }
                        }
                    }
                }
            }

            // Combine the current expression with the result using the operator specified in the expression tree
            if (currentExpression != null)
            {
                result = result == null ? currentExpression : CombineExpressions(result, currentExpression, expressionTree.Operator);
            }
        }


        return result;
    }

    private static Expression? BuildConditionExpression(Expression propertyExpression, ConstantExpression constantExpression, string operators)
    {
        // Build the appropriate expression based on the operator provided
        switch (operators)
        {
            case Constant.GreaterThanOrEqual:
                return Expression.GreaterThanOrEqual(propertyExpression, constantExpression);
            case Constant.LessThanOrEqual:
                return Expression.LessThanOrEqual(propertyExpression, constantExpression);
            case Constant.GreaterThan:
                return Expression.GreaterThan(propertyExpression, constantExpression);
            case Constant.LessThan:
                return Expression.LessThan(propertyExpression, constantExpression);
            case Constant.Equal:
                return Expression.Equal(propertyExpression, constantExpression);
            case Constant.InEqual:
                return Expression.NotEqual(propertyExpression, constantExpression);
            case Constant.Contains:
                var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                if (propertyExpression.Type != typeof(string) || methodInfo == null)
                {
                    return null;
                }

                return Expression.Call(propertyExpression, methodInfo, constantExpression);
            default:
                return null;
        }
    }

    private static Expression CombineExpressions(Expression left, Expression right, LogicalOperator? operatorType)
    {
        // Combine expressions using logical AND or OR based on the operator type
        return operatorType switch
        {
            LogicalOperator.and => Expression.AndAlso(left, right),
            LogicalOperator.or => Expression.OrElse(left, right),
            _ => throw new NotSupportedException($"The operator '{operatorType}' is not supported."),
        };
    }


    /// <summary>
    /// Determines if a property is valid for comparison operations such as greater than, less than, etc.
    /// </summary>
    /// <typeparam name="TResponse">The type of the object containing the property.</typeparam>
    /// <param name="type">The PropertyInfo of the property to check.</param>
    /// <param name="propertyName">The name of the property being checked.</param>
    /// <param name="comparisonOperator">The comparison operator being used (e.g., >=, <=, >, <).</param>
    /// <returns>True if the property is of a type that supports the specified comparison operations; otherwise, false.</returns>
    private static bool IsValidForComparison(PropertyInfo propertyInfo, string comparisonOperator)
    {
        var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
        if (comparisonOperator == Constant.GreaterThanOrEqual || comparisonOperator == Constant.LessThanOrEqual || comparisonOperator == Constant.GreaterThan || comparisonOperator == Constant.LessThan)
        {
            // Only allow comparison operations on numeric types, DateTime, and other specific types
            if (type == typeof(double) || type == typeof(DateTime) || type == typeof(long) || type == typeof(decimal) ||
                type == typeof(float) || type == typeof(int) || type == typeof(DateOnly) || type == typeof(TimeOnly))
            {
                return true;
            }
        }
        else
        {
            return true;
        }

        return false;
    }

    private static object? ConvertStringToType(string value, Type targetType)
    {
        return targetType switch
        {
            Type _ when targetType == typeof(int) => int.Parse(value),
            Type _ when targetType == typeof(DateOnly) => DateOnly.Parse(value),
            Type _ when targetType == typeof(DateTime) => DateTime.Parse(value),
            Type _ when targetType == typeof(double) => double.Parse(value),
            Type _ when targetType == typeof(decimal) => decimal.Parse(value),
            Type _ when targetType == typeof(long) => long.Parse(value),
            Type _ when targetType == typeof(float) => float.Parse(value),
            Type _ when targetType == typeof(bool) => bool.Parse(value),
            Type _ when targetType == typeof(Guid) => Guid.Parse(value),
            Type _ when targetType == typeof(string) => value, // No conversion needed for string
            _ => null
        };
    }
}

