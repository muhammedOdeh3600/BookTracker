using BookTracker.Data;
using BookTracker.Endpoints;
using BookTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.Services.AddScoped<BookService>();

builder.AddBookTrackerDb();

var app = builder.Build();


//app.MapGet("/", () => "Hello World!");

app.MapAuthorsEndpoint();
app.MapGenresEndpoint();
app.MapBooksEndpoint();
app.MigrateDb();

app.Run();