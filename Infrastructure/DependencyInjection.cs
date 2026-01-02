using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;
using Infrastructure.Persistence.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure; 

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, IdentityAuthService>();
        services.AddScoped<IClaimService,IdentityClaimService>();
        services.AddScoped<IEmailService,IdentityEmailService>();
        services.AddScoped<IPasswordService,IdentityPasswordService>();
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,ApplicationUserClaimsPrincipalFactory>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
