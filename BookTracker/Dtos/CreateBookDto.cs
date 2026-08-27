using System.ComponentModel.DataAnnotations;

namespace BookTracker.Dtos;

public record CreateBookDto(
    
    [Required] [StringLength(50)] string Title,
    [Required] [StringLength(50)] string Author,
    [Range(1, int.MaxValue)] int GenreId
    
    );