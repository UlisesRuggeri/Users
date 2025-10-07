
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDtos;

public class RegisterRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }

}
