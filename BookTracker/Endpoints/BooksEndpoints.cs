
using BookTracker.Dtos;
using BookTracker.Services;

namespace BookTracker.Endpoints;

public static class BooksEndpoints
{
    const string GetBookEndpoint = "GetBookById";
    public static void MapBooksEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/books");
        
        // GET
        group.MapGet("/", async (BookService bookService) =>

            {
                var books = await bookService.GetAllBooksAsync();
                return Results.Ok(books);
            }
            
            );

        group.MapGet("/{id}", async (int id, BookService bookService) =>
            {
                
                var book = await bookService.GetBookByIdAsync(id);
                
                return book is null
                    ? Results.NotFound() 
                    : Results.Ok(new BookDto(book.Id, book.Title, book.Author, book.Genre));
                
                
            }
                    
            ).WithName(GetBookEndpoint);
        
        // POST

        group.MapPost("/", async (CreateBookDto createdBookDto, BookService bookService) =>

            {
                var bookDto = await bookService.CreateBookAsync(createdBookDto);
                return Results.CreatedAtRoute("GetBookById", new { id = bookDto.Id }, bookDto);
            }
            
        );
        
        // PUT

        group.MapPut("/{id}", async (int id, UpdateBookDto updatedBookDto, BookService bookService)
                =>
            {
                var wasUpdated = await bookService.UpdateBookAsync(id, updatedBookDto);
                return wasUpdated
                    ? Results.NoContent()
                    : Results.NotFound();
            }
            
            );
        
        // DELETE

        group.MapDelete("/{id}", async (int id, BookService bookService)
                =>
            {
                await bookService.DeleteBookAsync(id);
                return Results.NoContent();
            }
            
            );

    }
}