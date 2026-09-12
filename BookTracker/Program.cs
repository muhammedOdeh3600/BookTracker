using BookTracker.Data;
using BookTracker.Endpoints;
using BookTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddMyServices();
builder.AddBookTrackerDb();

var app = builder.Build();


//app.MapGet("/", () => "Hello World!");

app.MapAuthorsEndpoint();
app.MapGenresEndpoint();
app.MapBooksEndpoint();
app.MigrateDb();

app.Run();