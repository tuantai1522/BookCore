using BookCore.Domain.Booking;
using BookCore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookCore.Infrastructure.Persistence.Repositories;

public sealed class BookRepository(BookCoreDbContext dbContext) : IBookRepository
{
    private readonly BookCoreDbContext _dbContext = dbContext;

    public void AddBook(Book book)
    {
        _dbContext.Set<Book>().Add(book);
    }

    public async Task<int> CountBooks()
        => await _dbContext.Set<Book>().CountAsync();


    public void DeleteBook(Book book)
    {
        _dbContext.Set<Book>().Remove(book);
    }

    public async Task<Book?> GetBookById(Guid BookId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Book>()
            .AsNoTracking()
            .Include(x => x.Authors)
            .Include(x => x.Reviews)
            .Include(x => x.BookPriceOffers)
            .FirstOrDefaultAsync(g => g.Id == BookId, cancellationToken);
    }

    public IQueryable<Book> GetQueryable()
    {
        return _dbContext.Set<Book>().AsQueryable();
    }
}
