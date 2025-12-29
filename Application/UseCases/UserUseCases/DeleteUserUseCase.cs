using Application.Common;
using Domain.Interfaces;

namespace Application.UseCases.UserUseCases;

public class DeleteUserUseCase
{
    private readonly IUserRepository _repo;
    public DeleteUserUseCase(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task DeleteUser(string userEmail) => await _repo.DeleteAsync(userEmail);
}
