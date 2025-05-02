using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.Cqrs;

public static class IServiceCollectionExtentions
{
    public static IServiceCollection AddWebsiteCqrs(this IServiceCollection services)
    {
        services.AddScoped<SetupCommand>();
        services.AddScoped<SetupQuery>();
        services.AddScoped<ScoreCommand>();
        services.AddScoped<ScoreQuery>();
        return services;
    }

    public static IServiceCollection AddAppCqrs(this IServiceCollection services)
    {
        services.AddScoped<SetupCommand>();
        services.AddScoped<SetupQuery>();
        services.AddScoped<ScoreCommand>();
        services.AddScoped<ScoreQuery>();
        services.AddScoped<AppQuery>();
        services.AddScoped<AppCommand>();
        return services;
    }
}
