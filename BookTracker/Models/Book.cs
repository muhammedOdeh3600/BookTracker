namespace BookTracker.Models;

public class Book
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    
    public Author? Author { get; set; }
    
    public int AuthorId { get; set; }
    
    public Genre? Genre { get; set; }
    
    public int GenreId { get; set; }
}