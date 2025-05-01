using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Rubriki.Website.Authentication;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddSecretCodeAuthentication(this AuthenticationBuilder builder, Action<SecretCodeAuthStateProvider.Options> getOptions)
    {
        var options = new SecretCodeAuthStateProvider.Options();
        getOptions(options);
        builder.Services.AddSingleton(options);
        builder.Services.AddScoped<SecretCodeAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider, SecretCodeAuthStateProvider>();
        return builder;
    }

    public static IServiceCollection AddSecretCodeAuthentication(this IServiceCollection services, Action<SecretCodeAuthStateProvider.Options> getOptions)
    {
        var options = new SecretCodeAuthStateProvider.Options();
        getOptions(options);
        services.AddSingleton(options);
#if false
        var authProvider = new SecretCodeAuthStateProvider(options);
        services.AddSingleton(authProvider);
        services.AddSingleton<AuthenticationStateProvider>(authProvider);
#else
        services.AddScoped<SecretCodeAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider, SecretCodeAuthStateProvider>();
        services.AddTransient<CookieService>();
#endif
        return services;
    }
}
