﻿
using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.UserInterfaces;

namespace Application.UseCases.UserUseCases;

public class GetCurrentUserUseCase
{
    private readonly IClaimService _claimService;

    public GetCurrentUserUseCase(IClaimService claimService) => _claimService = claimService;

    public async Task<Result<UserDto>> Execute()
    {
        var result = await _claimService.GetCurrentUser();

        if (!result.IsSucces)
            return Result<UserDto>.Failure(result.Error!);

        return Result<UserDto>.Succes(result.Value!);
    }
}
