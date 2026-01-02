

using Application.Common;
using Application.DTOs.UserDtos;
using Domain.Interfaces;
using Domain.Models;

namespace Application.UseCases.UserUseCases;

public class UpdateUserUseCase
{
    private readonly IUserRepository _repo;
    public UpdateUserUseCase(IUserRepository repo) => _repo = repo;

    public async Task<Result<bool>> UpdateUserAsync(UpdateUserRequest dto, CurrentUserDto currentUser, bool? isActive)
    {
        if(currentUser.Role == "admin") {
            bool aux = isActive ?? true;

            var Updates = new User
            {
                Id = dto.Id,
                Name = dto.name,
                IsActive = aux
            };
            var result = await _repo.UpdateAsync(Updates);
            if (result == false) return Result<bool>.Failure($"No se encontro el usuario con Id{dto.Id}");
        }
        else
        {
            var Updates = new User
            {
                Id = currentUser.Id,
                Name = dto.name,
            };
            var result = await _repo.UpdateAsync(Updates);
            if (result == false) return Result<bool>.Failure($"No se encontro el usuario con Id{dto.Id}");
        }

        return Result<bool>.Succes(value: true, message: "Se cambio el usuario");
    }
}
