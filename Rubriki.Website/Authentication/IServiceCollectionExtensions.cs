using Rubriki.Website.Authentication;

namespace Rubriki.Authentication;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServerAuthentication(this IServiceCollection services, Action<ServerAuthenticationService.Options> getOptions)
    {
        var options = new ServerAuthenticationService.Options();
        getOptions(options);
        services.AddSingleton(options);
        services.AddScoped<ISecretCodeAuthenticationService, ServerAuthenticationService>();
        services.AddTransient<CookieService>();

        return services;
    }
}
