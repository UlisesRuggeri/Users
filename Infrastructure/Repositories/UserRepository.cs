using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager) => _userManager = userManager;
    
   
    public async Task<User?> GetByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email) ;

        if (appUser == null) return null;

        var user = new User
        {
            Id = Guid.Parse(appUser.Id),
            Email = appUser.Email,
            Name = appUser.Name,
            EmailConfirmed = appUser.EmailConfirmed,
            IsActive = appUser.IsActive
        };
        return user;
    }
    public async Task<User?> GetByIdAsync(string id) {

        var appUser = await _userManager.FindByIdAsync(id);

        if (appUser == null) return null;

        var user = new User
        {
            Id = Guid.Parse(appUser.Id),
            Email = appUser.Email,
            Name = appUser.Name,
            EmailConfirmed = appUser.EmailConfirmed,
            IsActive = appUser.IsActive
        };
        return user;
    }

    public async Task AddAsync(User user, string password) {
        var appUser = new ApplicationUser
        {
            UserName = user.Email,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(appUser, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(". ", result.Errors.Select(e => e.Description));
            throw new UserCreationException(errors);
        }

        var anyUser = await _userManager.Users.AnyAsync();
        string role = anyUser ? "user" : "admin";
        await _userManager.AddToRoleAsync(appUser, role);
    }

    public async Task DeleteAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null) throw new UserNotFoundException(userEmail);

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) throw new ConflictException();
    }

    public async Task UpdateAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id!.ToString()) ?? throw new UserNotFoundException(user.Email!);

        appUser.Name = user.Name;
        appUser.IsActive = user.IsActive;
        appUser.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(appUser);
    }
}
