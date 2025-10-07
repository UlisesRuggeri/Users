
using Application.Common;
using Application.Interfaces.UserInterfaces;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;

public class IdentityPasswordService : IPasswordService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public IdentityPasswordService(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<Result<string>> GeneratePasswordResetToken(string email)
    {
        var user = await _userManager.FindByNameAsync(email);
        if (user == null)
            return Result<string>.Failure("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return Result<string>.Failure(token);
    }

    public async Task<Result<bool>> ResetPassword(int userId, string token, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            return Result<bool>.Failure(string.Join(",", result.Errors.Select(e => e.Description)));

        return Result<bool>.Succes(true);
    }

    public async Task<Result<bool>> ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        return Result<bool>.Succes(true);
    }
}
