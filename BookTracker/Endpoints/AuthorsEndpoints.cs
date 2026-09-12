using BookTracker.Dtos;
using BookTracker.Services;

namespace BookTracker.Endpoints;

public static class AuthorsEndpoints
{
    const string GetAuthorEndpoint = "GetAuthorById";

    public static void MapAuthorsEndpoint(this WebApplication app)
    {
        
        var group = app.MapGroup("/authors");
        
        // GET
        group.MapGet("/", async (AuthorService authorService) =>
            {
                var authors = await authorService.GetAllAuthorsAsync();
                Results.Ok(authors);
            }
            
        );

        group.MapGet("/{id}", async (int id, AuthorService authorService) =>
                {
                var author = await authorService.GetAuthorByIdAsync(id);
                return author is null
                    ? Results.NotFound()
                    : Results.Ok(
                        new AuthorDto(author.Id, author.Name)
                    );
            }
            
            )
            .WithName(GetAuthorEndpoint);
        
        
        // POST
        group.MapPost("/", async (CreateAuthorDto newAuthor, AuthorService authorService) =>
            {
                
                var authorDto = await authorService.CreateAuthorAsync(newAuthor);
                return Results.CreatedAtRoute(GetAuthorEndpoint, new {id = authorDto.Id},authorDto);

            }
        );
        
        //PUT
        group.MapPut("/{id}", async (int id, UpdateAuthorDto updatedAuthorDto, AuthorService authorService) =>
        {
            var updated = await authorService.UpdateAuthorAsync(id, updatedAuthorDto);

            return updated
                ? Results.NoContent()
                : Results.NotFound();
        });
        
        // DELETE
        group.MapDelete("/{id}", async (int id, AuthorService authorService)
            
            =>
        {
            var deleted =  await authorService.DeleteAuthorAsync(id);
            
            return deleted
                ? Results.NoContent()
                : Results.BadRequest(
                        "[Error]: A book or more have link with this author." +
                        " Delete linked books before trying again.."
                    );
        });

    }
    
}