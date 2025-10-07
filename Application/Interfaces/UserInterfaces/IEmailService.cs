
using Application.Common;

namespace Application.Interfaces.UserInterfaces;

public interface IEmailService
{
    Task<Result<string>> GenerateEmailConfirmationToken(int userId);
    Task<Result<bool>> SendEmailAsync(string to, string subject, string body);

}
