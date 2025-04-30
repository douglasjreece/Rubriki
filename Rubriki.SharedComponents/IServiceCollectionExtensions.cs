using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.SharedComponents;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSharedComponents(this IServiceCollection services)
    {
        services.AddScoped<ScoreEntryPanel.Model>();
        services.AddScoped<ResultsPanel.Model>();
        services.AddScoped<ContestantPanel.Model>();
        return services;
    }
}
