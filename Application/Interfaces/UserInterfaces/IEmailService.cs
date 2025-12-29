
using Application.Common;

namespace Application.Interfaces.UserInterfaces;

public interface IEmailService
{
    Task<Result<string>> GenerateEmailConfirmationToken(string userId);
    Task<Result<bool>> SendEmailAsync(string to, string subject, string body);
    Task ConfirmEmail(string id, string token);
}
