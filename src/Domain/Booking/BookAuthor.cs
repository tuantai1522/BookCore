using Shared;

namespace Domain.Booking
{
    public sealed class BookAuthor :
        Entity
    {
        public Guid BookId { get; private set; }

        public Guid AuthorId { get; private set; }

        private BookAuthor(Guid bookId, Guid authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
        }

        internal static Result<BookAuthor> Create(
            Guid bookId,
            Guid authorId)
        {
            return Result.Success(new BookAuthor(bookId, authorId));
        }
    }
}