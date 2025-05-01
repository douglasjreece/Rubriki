using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.Authentication;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSecretCodeAuthentication(this IServiceCollection services)
    {
        services.AddScoped<SecretCodeAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider, SecretCodeAuthStateProvider>();
        return services;
    }
}
