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
    public async Task<Result<string>> Login(LoginRequest dto)
    {
        var user = await _repo.GetByEmailAsync(dto.Email!);
        if (user == null) return Result<string>.Failure(error: "User not found");

        if (!user.IsActive) return Result<string>.Failure(error: "Usuario bloqueado");

        var loginResult = await _authService.Login(dto);
        if (!loginResult.IsSuccess)
            return Result<string>.Failure(error: loginResult.Error!);

        if (_authSettings.RequireConfirmedEmail && !user.EmailConfirmed)
            return Result<string>.Failure(value: user.Id.ToString(), error: "You have to confirm your email to continue");

        return Result<string>.Succes(user.Id.ToString());
    }
}
