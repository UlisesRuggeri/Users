using Application.Common;
using Application.DTOs.UserDtos;
using Domain.Models;

namespace Application.Interfaces.UserInterfaces;

public interface IAuthService
{
    Task<Result<bool>> Login(LoginRequest dto);
}
