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
        services.AddScoped<IStorageCommand, StorageCommand>();
        services.AddScoped<IStorageQuery, StorageQuery>();
        services.AddScoped<ISetupCommand, SetupCommand>();
        services.AddScoped<ISetupQuery, SetupQuery>();
        services.AddScoped<IScoreCommand, ScoreCommand>();
        services.AddScoped<IScoreQuery, ScoreQuery>();
        return services;
    }
}
