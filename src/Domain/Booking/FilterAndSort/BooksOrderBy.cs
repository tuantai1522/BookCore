using System.ComponentModel.DataAnnotations;

namespace BookCore.Domain.Booking.FilterAndSort;

public enum BooksOrderBy
{
    [Display(Name = "sort by...")]
    SimpleOrder = 0,
    [Display(Name = "Votes ↑")]
    ByVotes,
    [Display(Name = "Publication Date ↑")]
    ByPublicationDate,
    [Display(Name = "Price ↓")]
    ByPriceLowestFirst,
    [Display(Name = "Price ↑")]
    ByPriceHigestFirst
}
