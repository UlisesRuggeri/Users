
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

    public IdentityClaimService(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;

    }

    public Task<Result<UserDto>> GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity!.IsAuthenticated)
            return Task.FromResult(Result<UserDto>.Failure(error: "no hay un usuario autenticado"));

        var dto = new UserDto
        {
            Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Email = user.FindFirst(ClaimTypes.Email)?.Value,
            Name = user.FindFirst(ClaimTypes.Name)?.Value
        };

        return Task.FromResult(Result<UserDto>.Succes(value:dto, message: "SE COMPLETO getCurrentUser de identity, con validaciones .IsAuthenticated"));
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}