using BookTracker.Data;
using BookTracker.Dtos;
using BookTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Endpoints;

public static class AuthorsEndpoints
{
    const string GetAuthorEndpoint = "GetAuthorById";

    public static void MapAuthorsEndpoint(this WebApplication app)
    {
        
        var group = app.MapGroup("/authors");
        
        // GET
        group.MapGet("/", async (BookTrackerContext dbContext) => 
            await dbContext.Authors
                        .Select(author => new AuthorDto(author.Id, author.Name))
                        .AsNoTracking()
                        .ToListAsync()
        );

        group.MapGet("/{id}", async (int id, BookTrackerContext dbContext) =>
            {
                var author = await dbContext.Authors.FindAsync(id);
                return author is null
                    ? Results.NotFound()
                    : Results.Ok(
                        new AuthorDto(author.Id, author.Name)
                    );
            }
            
            )
            .WithName(GetAuthorEndpoint);
        
        
        // POST
        group.MapPost("/", async (CreateAuthorDto newAuthor, BookTrackerContext dbContext) =>
            {
                Author author = new()
                {
                    Name = newAuthor.Name
                    
                };
                dbContext.Authors.Add(author);
                await dbContext.SaveChangesAsync();

                AuthorDto authorDto = new AuthorDto(author.Id, author.Name);
                
                return Results.CreatedAtRoute(GetAuthorEndpoint, new {id = authorDto.Id},authorDto);

            }
        );
        
        //PUT
        group.MapPut("/{id}", async (int id, UpdateAuthorDto updatedAuthorDto, BookTrackerContext dbContext) =>
        {
            var author = await dbContext.Authors.FindAsync(id);

            if (author is null)
            {
                return Results.NotFound();
            }
            
            author.Name = updatedAuthorDto.Name;
            
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        });
        
        // DELETE
        group.MapDelete("/{id}", async (int id, BookTrackerContext dbContext)
            
            =>
        {
            await dbContext.Authors
                .Where(author => author.Id == id)
                .ExecuteDeleteAsync();
            return Results.NoContent(); 
        });

    }
    
}