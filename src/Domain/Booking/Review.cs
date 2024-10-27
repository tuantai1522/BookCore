using Shared;

namespace Domain.Booking;

public sealed class Review
    : Entity
{
    public string? VoterName { get; private set; }
    public int NumStars { get; private set; }
    public string Comment { get; private set; } = default!;

    private Review(string? voterName, int numStars, string comment)
    {
        VoterName = voterName;
        NumStars = numStars;
        Comment = comment;
    }

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
