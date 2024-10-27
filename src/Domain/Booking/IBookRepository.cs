using BookCore.Shared;
using Domain.Booking;

namespace BookCore.Domain.Booking;

public interface IBookRepository : IRepository<Book>
{
    public Task<Book?> GetBookById(Guid BookId, CancellationToken cancellationToken = default);
    public void AddBook(Book book);
    public void DeleteBook(Book book);
    public IQueryable<Book> GetQueryable();
    public Task<int> CountBooks();
}
