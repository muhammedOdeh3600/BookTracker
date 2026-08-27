using BookTracker.Data;
using BookTracker.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddBookTrackerDb();


var app = builder.Build();


//app.MapGet("/", () => "Hello World!");

app.MapAuthorsEndpoint();
app.MapBooksEndpoint();
app.MigrateDb();

app.Run();