using BookCore.Shared;
using Shared;

namespace BookCore.Domain.Authoring;

public interface IAuthorRepository : IRepository<Author, Guid>
{
    public Task<Author?> GetAuthorById(Guid AuthorId, CancellationToken cancellationToken = default);
    public void AddAuthor(Author author);
    public void DeleteAuthor(Author author);
    public IQueryable<Author> GetQueryable();
    public Task<int> CountAuthors();

}
