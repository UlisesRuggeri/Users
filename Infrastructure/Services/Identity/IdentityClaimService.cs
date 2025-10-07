
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
            return Task.FromResult(Result<UserDto>.Failure("There isn't an authenticated user"));

        var dto = new UserDto
        {
            Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Email = user.FindFirst(ClaimTypes.Email)?.Value,
            Name = user.FindFirst(ClaimTypes.Name)?.Value
        };

        return Task.FromResult(Result<UserDto>.Succes(dto));
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}