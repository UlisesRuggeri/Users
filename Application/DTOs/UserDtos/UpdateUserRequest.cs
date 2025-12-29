

using Application.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDtos;

public class UpdateUserRequest
{
    public Guid Id { get; set; }

    [Required]
    [MinLength(3)]
    [NotWhiteSpace]
    public string name { get; set; }
    public bool? IsActive { get; set; }
}
