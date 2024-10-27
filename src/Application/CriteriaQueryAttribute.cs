namespace BookCore.Application;


[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class CriteriaQueryAttribute : Attribute
{
    public bool NoFilter { get; }
    public bool NoSort { get; }

    public CriteriaQueryAttribute(bool noFilter = false, bool noSort = false)
    {
        NoFilter = noFilter;
        NoSort = noSort;
    }
}
