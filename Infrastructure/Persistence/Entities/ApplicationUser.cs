using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Entities;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
