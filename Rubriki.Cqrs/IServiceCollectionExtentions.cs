using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.Cqrs;

public static class IServiceCollectionExtentions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, Action<CqrsOptions> getOptions )
    {
        var options = new CqrsOptions();
        getOptions(options);
        services.AddSingleton(options);
        services.AddSingleton(options.StorageOptions);
        services.AddScoped<StorageCommand>();
        services.AddScoped<StorageQuery>();
        services.AddScoped<SetupCommand>();
        services.AddScoped<SetupQuery>();
        services.AddScoped<ScoreCommand>();
        services.AddScoped<ScoreQuery>();
        return services;
    }
}
