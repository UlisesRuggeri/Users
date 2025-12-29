
using Application.DTOs.UserDtos;
using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;
using Application.Common;
namespace Application.UseCases.UserUseCases;

public class ChangePasswordUseCase(IUserRepository repo,IPasswordService passwordService)
{
    private readonly IPasswordService _passwordService = passwordService;
    private readonly IUserRepository _repo = repo;

    public async Task<Result<bool>> ChangePassword(string userId,string token,ChangePasswordDto dto)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null)
            return Result<bool>.Failure(error: "User not found");

        var result = await _passwordService.ResetPassword(userId, token, dto.NewPassword!);
        if (!result.IsSuccess)
            return Result<bool>.Failure(error: result.Error!);

        return Result<bool>.Succes(value: true, message: "Se cambio la contrasenia correctamente");
    }
}
