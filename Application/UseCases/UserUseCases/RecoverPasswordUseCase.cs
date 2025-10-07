
using Application.Common;
using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;

namespace Application.UseCases.UserUseCases;

public class RecoverPasswordUseCase
{
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _repo;

    public RecoverPasswordUseCase( IUserRepository repo, IPasswordService passwordService, IEmailService emailService)
    {
        _repo = repo;
        _passwordService = passwordService;
        _emailService = emailService;
    }

    public async Task<Result<bool>> RecoverPassword(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null)
            return Result<bool>.Failure("User not found");

        var tokenResult = await _passwordService.GeneratePasswordResetToken(email);
        if (!tokenResult.IsSucces)
            return Result<bool>.Failure(tokenResult.Error!);

        var token = Uri.EscapeDataString(tokenResult.Value!);
        var link = $"https://miapp.com/reset-password?userId={user.Id}&token={token}";

        await _emailService.SendEmailAsync(user.Email!, "Reset your password",
            $"{user.Name}, Click to change the password: {link}");

        return Result<bool>.Succes(true);
    }
}

