using Shared;

namespace BookCore.Domain.Booking;

public sealed class Review(string? voterName, int numStars, string comment)
        : Entity<Guid>
{
    public string? VoterName { get; private set; } = voterName;
    public int NumStars { get; private set; } = numStars;
    public string Comment { get; private set; } = comment;

    public static Result<Review> Create(
        string voterName,
        int numStars,
        string comment
        )
    {
        // Validate
        if (numStars < 0)
        {
            return Result.Failure<Review>(Error.BadRequest("Star must be greater than 0"));
        }

        Review review = new(voterName, numStars, comment);

        return Result.Success(review);
    }
}
