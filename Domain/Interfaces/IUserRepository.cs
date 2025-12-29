using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(string id);
    Task AddAsync(User user, string password);

    Task DeleteAsync(string userEmail);
    Task UpdateAsync(User user);
}
