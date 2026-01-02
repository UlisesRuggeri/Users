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

    public async Task<Result<bool>> DeleteUser(string userEmail) {
        
        var result = await _repo.DeleteAsync(userEmail);
        if (!result) return Result<bool>.Failure($"No se encontro el usuario con email{userEmail}");
        return Result<bool>.Succes(value: true, message: "Se elimino el usuario correctamente");
    }
}
