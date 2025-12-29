

namespace Application.DTOs.UserDtos;

public class CurrentUserDto
{
    public Guid Id { get; set; }
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; }
}

