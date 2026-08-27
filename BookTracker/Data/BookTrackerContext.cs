using BookTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Data;

public class BookTrackerContext(DbContextOptions<BookTrackerContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Author> Authors => Set<Author>();
    
}