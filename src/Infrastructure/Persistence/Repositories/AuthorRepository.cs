using BookCore.Domain.Authoring;
using BookCore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookCore.Infrastructure.Persistence.Repositories;

public sealed class AuthorRepository(BookCoreDbContext dbContext) : IAuthorRepository
{
    private readonly BookCoreDbContext _dbContext = dbContext;

    public void AddAuthor(Author author)
    {
        _dbContext.Set<Author>().Add(author);
    }

    public async Task<int> CountAuthors()
        => await _dbContext.Set<Author>().CountAsync();

    public void DeleteAuthor(Author author)
    {
        _dbContext.Set<Author>().Remove(author);
    }

    public async Task<Author?> GetAuthorById(Guid AuthorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>()
            .AsNoTracking()
            .Include(x => x.Books)
            .FirstOrDefaultAsync(g => g.Id == AuthorId, cancellationToken);
    }

    public IQueryable<Author> GetQueryable()
    {
        return _dbContext.Set<Author>().AsQueryable();
    }
}
