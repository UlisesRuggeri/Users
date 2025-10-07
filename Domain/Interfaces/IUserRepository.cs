using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user, string password);
    Task UpdateAsync(User user);
}
