using Rubriki.Authentication;

namespace Rubriki.App.Authentication;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, Action<AppAuthenticationService.Options> getOptions)
    {
        var options = new AppAuthenticationService.Options();
        getOptions(options);
        services.AddSingleton(options);
        services.AddScoped<ISecretCodeAuthenticationService, AppAuthenticationService>();

        return services;
    }
}
