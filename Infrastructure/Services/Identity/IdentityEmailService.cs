
using Application.Common;
using Application.Interfaces.UserInterfaces;
using Domain.Exceptions;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services.Identity;

public class IdentityEmailService : IEmailService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SmtpClient _smtpClient;

    public IdentityEmailService( UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _smtpClient = new SmtpClient("proveedor") 
        {
            Port = puerto, 
            Credentials = new NetworkCredential("usuario", "contrasenia"),
            EnableSsl = true 
        };
    }

    public async Task<Result<string>> GenerateEmailConfirmationToken(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result<string>.Failure("User not found");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return Result<string>.Succes(token);
    }

    public async Task<Result<bool>> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var mail = new MailMessage("no-reply@User.com", to, subject, body);
            await _smtpClient.SendMailAsync(mail);
            return Result<bool>.Succes(true);
        }catch(Exception ex)
        {
            return Result<bool>.Failure(error: $"Error sending{ex.Message}");
        }
    }

    public async Task ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
            throw new Exception("token invalido o expirado");
    }
}
