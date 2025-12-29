using Application.Common;

namespace Application.Interfaces.UserInterfaces;

public interface IPasswordService
{
    Task<Result<bool>> ResetPassword(string userId, string token, string newPassword);
    Task<Result<string>> GeneratePasswordResetToken(string email);
    Task<Result<bool>> ChangePassword(string userId, string currentPassword, string newPassword);


}
