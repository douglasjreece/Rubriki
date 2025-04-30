using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.Cqrs;

public static class IServiceCollectionExtentions
{
    public static IServiceCollection AddWebsiteCqrs(this IServiceCollection services)
    {
        services.AddScoped<SetupCommand>();
        services.AddScoped<AdminCommand>();
        services.AddScoped<AdminQuery>();
        services.AddScoped<ClientCommand>();
        services.AddScoped<ClientQuery>();
        return services;
    }

    public static IServiceCollection AddAppCqrs(this IServiceCollection services)
    {
        services.AddScoped<SetupCommand>();
        services.AddScoped<ClientCommand>();
        services.AddScoped<ClientQuery>();
        services.AddScoped<AppQuery>();
        return services;
    }
}
