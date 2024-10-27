using System.ComponentModel.DataAnnotations;

namespace BookCore.Domain.Booking.FilterAndSort;

public enum BooksFilterBy
{
    [Display(Name = "All")]
    NoFilter = 0,
    [Display(Name = "By Votes...")]
    ByVotes,
    [Display(Name = "By Year published...")]
    ByPublicationYear
}
