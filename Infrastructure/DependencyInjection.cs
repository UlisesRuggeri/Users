using Application.Interfaces.UserInterfaces;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services.Identity;
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

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
