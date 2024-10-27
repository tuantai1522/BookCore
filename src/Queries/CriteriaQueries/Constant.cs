namespace BookCore.Application.Queries.CriteriaQueries;

public static class Constant
{
    public const string Equal = "==";
    public const string InEqual = "!=";
    public const string GreaterThan = ">";
    public const string LessThan = "<";
    public const string GreaterThanOrEqual = ">=";
    public const string LessThanOrEqual = "<=";
    public const string Contains = "@=";

    public const string ConditionPatternRegex = @"^(.*?)(@=|>=|<=|==|>|<)(.*)$";
}
