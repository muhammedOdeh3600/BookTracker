using BookTracker.Data;
using BookTracker.Dtos;
using BookTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Endpoints;

public static class GenresEndpoints
{
    
    const string GetGenresEndpoint = "GetGenresById";

    public static void MapGenresEndpoint(this WebApplication app)
    {

        var group = app.MapGroup("/genres");

        // GET
        group.MapGet("/", async (BookTrackerContext dbContext) =>

            await dbContext.Genres
                .Select(genre => new GenreDto(genre.Id, genre.Name))
                .AsNoTracking()
                .ToListAsync()

        );

        group.MapGet("/{id}", async (int id, BookTrackerContext dbContext) =>
            {
                var genre = await  dbContext.Genres.FindAsync(id);
                
                return genre is null 
                    ? Results.NotFound() 
                    : Results.Ok(new GenreDto(genre.Id, genre.Name));
            }
        ).WithName(GetGenresEndpoint);
        
        
        //POST

        group.MapPost("/", async (CreateGenreDto createdGenredto, BookTrackerContext dbContext) =>

            {
                Genre genre = new()
                {
                    Name = createdGenredto.Name
                };
                
                dbContext.Genres.Add(genre);
                await dbContext.SaveChangesAsync();
                
                GenreDto genreDto = new GenreDto(genre.Id, genre.Name);
                return Results.CreatedAtRoute(GetGenresEndpoint, new {id = genre.Id}, genreDto);
            }
            
            );
        
        // PUT

        group.MapPut("/{id}", async (int id, UpdateGenreDto updatedGenreDto, BookTrackerContext dbContext)
                =>
            {
                var genre = await dbContext.Genres.FindAsync(id);

                if (genre is null)
                {
                    return Results.NotFound();
                }
                
                genre.Name = updatedGenreDto.Name;
                
                await dbContext.SaveChangesAsync();
                
                return Results.NoContent();
            }
            
            );
        
        // DELETE

        group.MapDelete("/{id}", async (int id, BookTrackerContext dbContext)
            =>
            {
                
                var hasLinkedBooks = await dbContext.Books.AnyAsync(b => b.GenreId == id);

                if (hasLinkedBooks)
                {
                    return Results.BadRequest( 
                        "[Error]: A book or more have link with this genre" +
                             " Delete linked books before trying again.");
                }
                
                await dbContext.Genres
                    .Where(genre => genre.Id == id)
                    .ExecuteDeleteAsync();
                return Results.NoContent();
            }
            
            
            );

    }
}