using System.ComponentModel.DataAnnotations;

namespace BookTracker.Dtos;

public record UpdateAuthorDto(
    
    [Required][StringLength(50)]string Name
    
    );