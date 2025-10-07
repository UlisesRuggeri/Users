using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.UserInterfaces;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;

public class IdentityAuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityAuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result<bool>> Login(LoginRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email!);
        if (user == null) return Result<bool>.Failure("User not found");


        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: true, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Result<bool>.Failure("Invalid credentials");

        return Result<bool>.Succes(true);
    }
}
