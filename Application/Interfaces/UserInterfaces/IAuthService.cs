using Application.Common;
using Application.DTOs.UserDtos;

namespace Application.Interfaces.UserInterfaces;

public interface IAuthService
{
    Task<Result<bool>> Login(LoginRequest dto);
}
