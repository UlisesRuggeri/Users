
using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.UseCases.UserUseCases;

public class LoginUseCase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _repo;
    private readonly AuthSettings _authSettings;

    public LoginUseCase(IOptions<AuthSettings> authSettings,IAuthService authService, IUserRepository repo)
    {
        _authSettings = authSettings.Value;
        _authService = authService;
        _repo = repo;
    }
    public async Task<Result<bool>> Login(LoginRequest dto)
    {
        var user = await _repo.GetByEmailAsync(dto.Email!);
        if (user == null) return Result<bool>.Failure("User not found");

        if (!user.IsActive)
            return Result<bool>.Failure("Usuario blocked");

        if (_authSettings.RequireConfirmedEmail && !user.EmailConfirmed)
            return Result<bool>.Failure("You have to confirm your email to continue");

        var loginResult = await _authService.Login(dto);
        if (!loginResult.IsSucces)
            return Result<bool>.Failure(loginResult.Error!);

        return Result<bool>.Succes(true);
    }
}
