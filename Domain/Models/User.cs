namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string? Email { get; set; } 
    public string? Name { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsActive { get; set; }
    
}
