using Rubriki.App.Components.Pages;

namespace Rubriki.App.Components;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddModels(this IServiceCollection services)
    {
        services.AddTransient<SetupPage.Model>();
        services.AddTransient<SubmitScoresPage.Model>();
        return services;
    }
}
