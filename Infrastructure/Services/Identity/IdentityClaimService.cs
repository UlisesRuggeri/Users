
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

        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Result<UserDto>.Failure(error: "No se encontró el Id del usuario");

        var appUser = await _userManager.FindByIdAsync(userId);
        if (appUser == null)
            return Result<UserDto>.Failure(error: "Usuario no encontrado");

        var dto = new UserDto
        {
            Id = appUser.Id.ToString(),
            Email = appUser.Email,
            Name = appUser.Name,
            Role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault() ?? "user",
            IsActive = appUser.IsActive
        };

        return Result<UserDto>.Succes(value: dto, message: "SE COMPLETO getCurrentUser de identity, con validaciones .IsAuthenticated");
    }


    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}