
using Application.Common;
using Application.DTOs.UserDtos;
using Domain.Interfaces;
using Domain.Models;

namespace Application.UseCases.UserUseCases;

public class RegisterUseCase
{
    private readonly IUserRepository _repo;

    public RegisterUseCase(IUserRepository repo) => _repo = repo;
    public async Task<Result<bool>> Register(RegisterRequest dto)
    {
        var user = await _repo.GetByEmailAsync(dto.Email!);
        if (user != null) return Result<bool>.Failure("Este Email ya esta registrado");

        var newUser = new User
        {
            Email = dto.Email,
            Name = dto.Name
        };
        await _repo.AddAsync(newUser, dto.Password!);
        return Result<bool>.Succes(true);
    }
}
