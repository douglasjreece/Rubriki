using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.Cqrs;

public static class IServiceCollectionExtentions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddScoped<AdminCommand>();
        services.AddScoped<AdminQuery>();
        services.AddScoped<ClientCommand>();
        services.AddScoped<ClientQuery>();
        return services;
    }
}
