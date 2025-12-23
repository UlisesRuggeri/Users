
using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;

namespace Application.UseCases.UserUseCases;

public class ChangePasswordUseCase
{
    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _repo;

    public ChangePasswordUseCase(
        IUserRepository repo,
        IPasswordService passwordService)
    {
        _repo = repo;
        _passwordService = passwordService;
    }

    public async Task<Result<bool>> ChangePassword(ChangePasswordDto dto)
    {
        var user = await _repo.GetByIdAsync(dto.UserId);
        if (user == null)
            return Result<bool>.Failure(error: "User not found");

        var result = await _passwordService.ResetPassword(dto.UserId, dto.Token!, dto.NewPassword!);
        if (!result.IsSucces)
            return Result<bool>.Failure(error: result.Error!);

        return Result<bool>.Succes(value: true, message: "Se cambio la contrasenia correctamente");
    }
}
