using BookCore.Domain.Booking;
using BookCore.Shared;
using Shared;

namespace BookCore.Domain.Authoring;

public sealed class Author(string name)
    : AggregateRoot<Author, Guid>
{
    public string Name { get; private set; } = name;

    //-----------------------------------------------
    //relationships

    public IReadOnlyCollection<Book> Books => [.. _books];

    private ICollection<Book> _books = [];

    //methods
    public static Result<Author> Create(string name)
    {
        Author author = new(name);

        return Result.Success(author);
    }

    public void AddBook(Book book)
    {
        _books.Add(book);
    }
}