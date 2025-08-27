using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;
using SmartEM.Application.Utils.SecurityProvider;

namespace SmartEM.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CustomLoginCommandValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CustomLoginCommand>());
        services.AddScoped(provider => configuration.GetRequiredSection(ApplicationConstants.JWT_CONFIGURATION).Get<JWTConfiguration>()!);
        services.AddScoped(provider => configuration.GetRequiredSection(ApplicationConstants.TOKEN_EXPIRY).Get<TokenExpiry>()!);
        services.AddScoped(provider => configuration.GetRequiredSection(ApplicationConstants.DEFAULT_USER_CONFIGURATION).Get<DefaultUserConfiguration>()!);
        services.AddScoped<IAccessManager, AccessManager>();
        services.AddScoped<IJWTProvider, JWTProvider>();
        services.AddScoped<IUserIdentity, UserIdentity>();
        return services;
    }
}
