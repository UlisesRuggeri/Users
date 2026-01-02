using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(string id);
    Task AddAsync(User user, string password);

    Task<bool> DeleteAsync(string userEmail);
    Task<bool> UpdateAsync(User user);
}
