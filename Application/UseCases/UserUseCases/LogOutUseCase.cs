
using Application.Interfaces.UserInterfaces;

namespace Application.UseCases.UserUseCases;

public class LogOutUseCase
{
    private readonly IClaimService _claimService;
    public LogOutUseCase(IClaimService claimService) => _claimService = claimService;

    public async Task LogOut() => await _claimService.SignOutAsync();
}
