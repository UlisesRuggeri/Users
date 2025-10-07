
using Application.Common;
using Application.DTOs.UserDtos;

namespace Application.Interfaces.UserInterfaces;

public interface IClaimService
{
    Task SignOutAsync();
    Task<Result<UserDto>> GetCurrentUser();
}
