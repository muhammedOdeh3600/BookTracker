using BookTracker.Models;
using BookTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Data;

public static class DataExtensions
{

    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<BookTrackerContext>();
        dbContext.Database.Migrate();
    }
    
    public static void AddBookTrackerDb(this WebApplicationBuilder builder)
    {
        
        var connString = builder.Configuration.GetConnectionString("BookTracker");

        builder.Services.AddSqlite<BookTrackerContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        
                        new Genre{Name = "Mystery"},
                        new Genre{Name = "Fantasy"},
                        new Genre{Name = "Comedy"},
                        new Genre{Name = "Romance"},
                        new Genre{Name = "Sci-Fi"},
                        new Genre{Name = "Action"},
                        new Genre{Name = "Culture"},
                        new Genre{Name = "Literature"},
                        new Genre{Name = "Crime"},
                        new Genre{Name = "Horror"}
                        
                        );

                }
                context.SaveChanges();
            })
        );
    }


    public static void AddMyServices(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddValidation();
        builder.Services.AddScoped<BookService>();
        builder.Services.AddScoped<AuthorService>();
        
    }
}