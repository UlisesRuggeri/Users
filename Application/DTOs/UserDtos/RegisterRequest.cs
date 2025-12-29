
using Application.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDtos;

public class RegisterRequest
{
    [Required]
    [MaxLength(50)]
    [NotWhiteSpace]
    public string Email { get; set; }
    [Required]
    [MinLength(6)]
    [NotWhiteSpace]
    public string Password { get; set; }
    [Required]
    [MinLength(3)]
    [NotWhiteSpace]
    public string? Name { get; set; }
}
