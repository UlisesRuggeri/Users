using Application.Interfaces.UserInterfaces;
using Application.UseCases.UserUseCases;
using Microsoft.Extensions.DependencyInjection;


namespace EsteroidesToDo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //User use cases:
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<ConfirmEmailUseCase>();
        services.AddScoped<RecoverPasswordUseCase>();
        services.AddScoped<ChangePasswordUseCase>();
        services.AddScoped<GetCurrentUserUseCase>();
        services.AddScoped<LogOutUseCase>();

        return services;
    }
}
