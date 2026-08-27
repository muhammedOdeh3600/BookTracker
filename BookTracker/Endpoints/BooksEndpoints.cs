using BookTracker.Data;
using BookTracker.Dtos;
using BookTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Endpoints;

public static class BooksEndpoints
{
    const string GetBookEndpoint = "GetBookById";
    public static void MapBooksEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/books");
        
        // GET
        group.MapGet("/", async (BookTrackerContext dbContext) => 
            
            await  dbContext.Books
                .Select(book => new BookDto(book.Id, book.Title,
                    book.Author!.Name,
                    book.Genre!.Name))
                .AsNoTracking()
                .ToListAsync()
            
            );

        group.MapGet("/{id}", async (int id, BookTrackerContext dbContext) =>
            {
                var book = await dbContext.Books.Where(b => b.Id == id)
                    .Select(book => new BookDto(book.Id,
                        book.Title,
                        book.Author!.Name,
                        book.Genre!.Name)
                    ).FirstOrDefaultAsync();

                return book is null
                    ? Results.NotFound() 
                    : Results.Ok(new BookDto(book.Id, book.Title, book.Author, book.Genre));
                
                
            }
                    
            ).WithName(GetBookEndpoint);
        
        // POST

        group.MapPost("/", async (CreateBookDto createdBookDto, BookTrackerContext dbContext) =>

            {
                var author = await dbContext.Authors
                    .FirstOrDefaultAsync(a => a.Name.Trim().ToLower() == createdBookDto.Author.Trim().ToLower());
                var genre = await dbContext.Genres.FindAsync(createdBookDto.GenreId);
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
                
                dbContext.Books.Add(book);
                await dbContext.SaveChangesAsync();
                
                BookDto bookDto = new BookDto(book.Id, book.Title, createdBookDto.Author, genre!.Name );
                return Results.CreatedAtRoute(GetBookEndpoint, new { id = book.Id }, bookDto);

            }
        );
        
        // PUT

        group.MapPut("/{id}", async (int id, UpdateBookDto updatedBookDto, BookTrackerContext dbContext)
                =>
            {
                var book =  await dbContext.Books.
                        FirstOrDefaultAsync(b => b.Id == id);
                if (book is null)
                {
                    return Results.NotFound();
                }
                var author = await dbContext.Authors
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
                
                await dbContext.SaveChangesAsync();
                
                return Results.NoContent();
            }
            
            );
        
        // DELETE

        group.MapDelete("/{id}", async (int id, BookTrackerContext dbContext)
                =>
            {
                
                //var authorHasOtherBooks = await dbContext.Authors.
                
                await dbContext.Books.Where(b => b.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            }
            
            );

    }
}