using System.ComponentModel.DataAnnotations;

namespace BookTracker.Dtos;

public record UpdateGenreDto(
    
    [Required] [StringLength(50)] string Name
    
    );