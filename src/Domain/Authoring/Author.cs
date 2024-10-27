using Domain.Booking;
using Shared;

namespace Domain.Authoring;

public sealed class Author :
    Entity, IAggregateRoot
{
    public string Name { get; private set; } = default!;

    //-----------------------------------------------
    //relationships

    public IReadOnlyCollection<BookAuthor> Books => [.. _books];

    private ICollection<BookAuthor> _books = [];

    //constructors
    private Author(string name)
    {
        Name = name;
    }
    //methods
    public static Result<Author> Create(
        string name
        )
    {
        Author author = new(name);

        return Result.Success<Author>(author);
    }

    public void AddBook(BookAuthor book)
    {
        Result<BookAuthor> bookAuthor = BookAuthor.Create(book.Id, Id);

        _books.Add(bookAuthor.Value);
    }
}