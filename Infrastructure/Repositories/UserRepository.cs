using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");

        var user = new User
        {
            Email = appUser.Email,
            Name = appUser.Name
        };
        return user;
    }
    public async Task<User> GetByIdAsync(int id) {

        var appUser = await _userManager.FindByIdAsync($"{id}") ?? throw new Exception("User not found");
        var user = new User
        {
            Email = appUser.Email,
            Name = appUser.Name
        };
        return user;
    }

    public async Task AddAsync(User user, string password) {
        var appUser = new ApplicationUser
        {
            UserName = user.Email,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = DateTime.UtcNow
        };

        await _userManager.CreateAsync(appUser, password);
        await _userManager.AddToRoleAsync(appUser, "user");
    }

    public async Task UpdateAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id!.ToString()) ?? throw new Exception("User not found");

        appUser.Email = user.Email;
        appUser.Name = user.Name;
        appUser.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(appUser);
    }
}
