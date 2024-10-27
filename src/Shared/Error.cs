namespace Shared;

public record Error
{
    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }
    
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public static Error NotFound(string description) =>
        new("404", description, ErrorType.NotFound);

    public static Error Problem(string description) =>
        new("500", description, ErrorType.Problem);

    public static Error Conflict(string description) =>
        new("409", description, ErrorType.Conflict);

    public static Error BadRequest(string description) =>
        new("400", description, ErrorType.BadRequest);
}
