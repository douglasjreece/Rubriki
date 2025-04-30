using Rubriki.Website.Components.Pages;

namespace Rubriki.Website.Components;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddWebsiteModels(this IServiceCollection services)
    {
        services.AddScoped<AdminPage.Model>();
        return services;
    }
}
