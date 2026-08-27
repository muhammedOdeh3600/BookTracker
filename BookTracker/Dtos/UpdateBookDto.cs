using System.ComponentModel.DataAnnotations;

namespace BookTracker.Dtos;

public record UpdateBookDto(
    
    [Required] [StringLength(50)] string Title,
    [Range(1, int.MaxValue)] int AuthorId,
    [Range(1, int.MaxValue)] int GenreId
    
    );