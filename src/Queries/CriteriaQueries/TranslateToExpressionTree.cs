using System.Text;

namespace BookCore.Application.Queries.CriteriaQueries;

public static class TranslateToExpressionTree
{
    public static ExpressionTree? Translate(string? input)
    {
        if (input != null)
        {
            // Remove outermost parentheses, if any.
            input = input.Trim();
            if (input.StartsWith("(") && input.EndsWith(")"))
            {
                input = input.Substring(1, input.Length - 2);
            }

            var andParts = SplitByOperator(input, "and");

            if (andParts.Count > 1)
            {
                var logicalOperator = Enum.TryParse("and", out LogicalOperator operators);
                return new ExpressionTree
                {
                    Operator = operators,
                    OperationExpressions = andParts.Select(ParseToOperationExpression).ToList()
                };
            }

            var orParts = SplitByOperator(input, "or");

            if (orParts.Count > 1)
            {
                var logicalOperator = Enum.TryParse("or", out LogicalOperator operators);
                return new ExpressionTree
                {
                    Operator = operators,
                    OperationExpressions = orParts.Select(ParseToOperationExpression).ToList()
                };
            }
        }
        return null;
    }

    private static OperationExpression ParseToOperationExpression(string input)
    {
        // If there are inner conditions, parse them as a subexpression
        if (input.ToLower().Contains("and") || input.ToLower().Contains("or"))
        {
            return new OperationExpression
            {
                SubExpression = Translate(input)
            };
        }

        return new OperationExpression
        {
            Condition = input.Trim()
        };
    }

    private static List<string> SplitByOperator(string input, string operatorKeyword)
    {
        var parts = new List<string>();
        var sb = new StringBuilder();
        var parenthesesLevel = 0;

        for (int i = 0; i < input.Length; i++)
        {
            var currentChar = input[i];

            if (currentChar == '(')
            {
                parenthesesLevel++;
            }
            else if (currentChar == ')')
            {
                parenthesesLevel--;
            }

            if (parenthesesLevel == 0 && input.ToLower().Substring(i).StartsWith(operatorKeyword.ToLower()))
            {
                parts.Add(sb.ToString().Trim());
                sb.Clear();
                i += operatorKeyword.Length - 1;
            }
            else
            {
                sb.Append(currentChar);
            }
        }

        if (sb.Length > 0)
        {
            parts.Add(sb.ToString().Trim());
        }

        return parts;
    }
}
