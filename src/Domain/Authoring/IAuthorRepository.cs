using BookCore.Shared;
using Domain.Authoring;

namespace BookCore.Domain.Authoring;

public interface IAuthorRepository : IRepository<Author>
{
    public Task<Author?> GetAuthorById(Guid AuthorId, CancellationToken cancellationToken = default);
    public void AddAuthor(Author author);
    public void DeleteAuthor(Author author);
}
