using System.ComponentModel.DataAnnotations;

namespace BookTracker.Dtos;

public record CreateGenreDto(
    
    [Required] [StringLength(50)] string Name
    
    );