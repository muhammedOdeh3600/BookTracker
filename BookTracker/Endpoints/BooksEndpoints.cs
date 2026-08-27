namespace BookTracker.Endpoints;

public static class BooksEndpoints
{
    const string GetBookEndpoint = "GetBookById";
    public static void MapBooksEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/books");
        
        
    }
}