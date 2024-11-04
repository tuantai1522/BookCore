using BookCore.Domain.Authoring;
using BookCore.Domain.Booking;
using BookCore.Domain.Ordering;
using Microsoft.EntityFrameworkCore;

namespace BookCore.Infrastructure.Persistence.Contexts;

public class BookCoreDbContext(DbContextOptions<BookCoreDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PriceOffer> PriceOffers { get; set; }
}
