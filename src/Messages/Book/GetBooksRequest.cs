using BookCore.Domain.Booking.FilterAndSort;
using MediatR;
using Shared;

namespace BookCore.Messages.Book;

public class GetBooksRequest
    : IRequest<Result<GetBooksResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 16;

    public BooksOrderBy BooksOrderBy { get; set; }
    public BooksFilterBy BooksFilterBy { get; set; }
}
