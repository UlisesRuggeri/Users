
using Application.Common;
using Application.Interfaces.UserInterfaces;
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
        _smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io") 
        {
            Port = 2525, 
            Credentials = new NetworkCredential("838f32cdae17b5", "73a304b640bce7"),
            EnableSsl = true 
        };
    }

    public async Task<Result<string>> GenerateEmailConfirmationToken(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<string>.Failure("User not found");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return Result<string>.Succes(token);
    }

    public async Task<Result<bool>> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var mail = new MailMessage("no-reply@miapp.com", to, subject, body);
            await _smtpClient.SendMailAsync(mail);
            return Result<bool>.Succes(true);
        }catch(Exception ex)
        {
            return Result<bool>.Failure(error: $"Error sending{ex.Message}");
        }
    }
}
