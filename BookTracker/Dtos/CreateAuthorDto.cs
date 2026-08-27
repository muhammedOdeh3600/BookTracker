using System.ComponentModel.DataAnnotations;

namespace BookTracker.Dtos;

public record CreateAuthorDto(
    
    [Required] [StringLength(50)] string Name
    
    );