
using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.UserInterfaces;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services.Identity;

public class IdentityClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    public IdentityClaimService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
    }

    public async Task<Result<UserDto>> GetCurrentUser()
    {
        var userClaims = _httpContextAccessor.HttpContext?.User;

        if (userClaims == null || !userClaims.Identity!.IsAuthenticated) return Result<UserDto>.Failure(error: "no hay un usuario autenticado");
        var isActiveClaim = userClaims.FindFirst("IsActive")?.Value;
        bool isActive = bool.TryParse(isActiveClaim, out var result) && result;

        var dto = new UserDto
        {
            Id = userClaims.FindFirst("Id")?.Value.ToString(),
            Email = userClaims.FindFirst("Email")?.Value,
            Name = userClaims.FindFirst("Name")?.Value,
            Role = userClaims.FindFirst("Role")?.Value,
            IsActive = isActive
        };

        return Result<UserDto>.Succes(value: dto, message: "SE COMPLETO getCurrentUser de identity, con validaciones .IsAuthenticated");
    }


    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}