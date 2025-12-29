

using Application.Common;
using Application.DTOs.UserDtos;
using Domain.Interfaces;
using Domain.Models;
using System.Runtime.Intrinsics.X86;

namespace Application.UseCases.UserUseCases;

public class UpdateUserUseCase
{
    private readonly IUserRepository _repo;
    public UpdateUserUseCase(IUserRepository repo) => _repo = repo;

    public async Task UpdateUserAsync(UpdateUserRequest dto, CurrentUserDto currentUser, bool? isActive)
    {
        if(currentUser.Role == "admin") {
            bool aux = isActive ?? true;

            var Updates = new User
            {
                Id = dto.Id,
                Name = dto.name,
                IsActive = aux
            };
            await _repo.UpdateAsync(Updates);
        }
        else
        {
            var Updates = new User
            {
                Id = currentUser.Id,
                Name = dto.name,
            };
            await _repo.UpdateAsync(Updates);
        }
    }
}
