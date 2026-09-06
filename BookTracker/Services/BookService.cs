using BookTracker.Data;
using BookTracker.Dtos;
using BookTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Services;

public class BookService
{
    private readonly BookTrackerContext _dbContext;
    
    public BookService(BookTrackerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        return await _dbContext.Books
            .Select(book => new BookDto(book.Id, book.Title,
                book.Author!.Name,
                book.Genre!.Name))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        return await _dbContext.Books.Where(b => b.Id == id)
            .Select(book => new BookDto(book.Id,
                book.Title,
                book.Author!.Name,
                book.Genre!.Name)
            ).FirstOrDefaultAsync();
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto createdBookDto)
    {
        
        var author = await _dbContext.Authors
            .FirstOrDefaultAsync(a => a.Name.Trim().ToLower() == createdBookDto.Author.Trim().ToLower());
        var genre = await _dbContext.Genres.FindAsync(createdBookDto.GenreId);
        if (author is null)
        {
            author = new Author
            {
                Name = createdBookDto.Author
            };
        }

        Book book = new()
        {
            Title = createdBookDto.Title,
            Author = author,
            GenreId = createdBookDto.GenreId
        };
                
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
                
        BookDto bookDto = new BookDto(book.Id, book.Title, createdBookDto.Author, genre!.Name );
        return bookDto;
        
    }

    public async Task<bool> UpdateBookAsync(int id, UpdateBookDto updatedBookDto)
    {
        var book =  await _dbContext.Books.
            FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
        {
            return false;
        }
        var author = await _dbContext.Authors
            .FirstOrDefaultAsync(a => a.Name.Trim().ToLower() == updatedBookDto.Author.Trim().ToLower());
        if (author is null)
        {
            author = new Author
            {
                Name = updatedBookDto.Author
            };
        }
        book.Title = updatedBookDto.Title;
        book.Author = author;
        book.GenreId = updatedBookDto.GenreId;
                
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);

        var authorId = book.AuthorId;
                
        await _dbContext.Books.Where(b => b.Id == id).ExecuteDeleteAsync();
                
        var authorHasOtherBooks = await _dbContext.Books
            .AnyAsync(a => a.AuthorId == authorId);

        if (!authorHasOtherBooks)
        {
            await _dbContext.Authors.Where(a => a.Id == authorId).ExecuteDeleteAsync();
        }
    }
    
}