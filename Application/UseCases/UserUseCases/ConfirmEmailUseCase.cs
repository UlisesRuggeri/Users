
using Application.Common;
using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;

namespace Application.UseCases.UserUseCases;

public class ConfirmEmailUseCase
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _repo;

    public ConfirmEmailUseCase(IEmailService emailService, IUserRepository repo)
    {
        _emailService = emailService;
        _repo = repo;
    }

    public async Task<Result<bool>> SendConfirmEmail(string userId)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null)
            return Result<bool>.Failure(error: "Usuario No encontrado");

        var tokenResult = await _emailService.GenerateEmailConfirmationToken(userId);
        if (!tokenResult.IsSuccess)
            return Result<bool>.Failure(error: tokenResult.Error!);

        var token = Uri.EscapeDataString(tokenResult.Value!); 
        var confirmationLink = $"http://localhost:5178/Api/Users/confirm-email?userId={userId}&token={token}";

        var subject = "Confirmar email";
        var body = $@"
        Hola {user.Name}!!
        aca podes poner el texto que quieras, pero siempre dejando el
        {confirmationLink}
        ";
        var sendResult = await _emailService.SendEmailAsync(user.Email!, subject, body);
        if (!sendResult.IsSuccess)
            return Result<bool>.Failure(error: sendResult.Error!);

        return Result<bool>.Succes(value: true, message: "Email de confirmacion enviado exitosamente");
    }

    public async Task ConfirmEmail(string id, string token)
    {
        await _emailService.ConfirmEmail(id, token);
    }
}
