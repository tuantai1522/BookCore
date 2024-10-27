namespace BookCore.Application.Queries.CriteriaQueries;

public class ExpressionTree
{
    public LogicalOperator? Operator { get; set; }
    public List<OperationExpression>? OperationExpressions { get; set; }
}

public class OperationExpression
{
    public ExpressionTree? SubExpression { get; set; }
    public string? Condition { get; set; }
}
