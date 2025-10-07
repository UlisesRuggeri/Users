﻿
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

    public async Task<Result<bool>> ConfirmEmail(int userId)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null)
            return Result<bool>.Failure("User not found");

        var tokenResult = await _emailService.GenerateEmailConfirmationToken(userId);
        if (!tokenResult.IsSucces)
            return Result<bool>.Failure(tokenResult.Error!);

        var token = Uri.EscapeDataString(tokenResult.Value!); //para que sea seguro en url
        var confirmationLink = $"https://miapp.com/confirm-email?userId={userId}&token={token}";

        var subject = "Confirm your email";
        var body = $@"
        Hola {user.Name}!!
        aca podes poner el texto que quieras, pero siempre dejando el
        {confirmationLink}
        ";
        var sendResult = await _emailService.SendEmailAsync(user.Email!, subject, body);
        if (!sendResult.IsSucces)
            return Result<bool>.Failure(sendResult.Error!);

        return Result<bool>.Succes(true);

    }
}
