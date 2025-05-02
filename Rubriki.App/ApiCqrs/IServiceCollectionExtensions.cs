namespace Rubriki.App.ApiCqrs;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApiCqrs(this IServiceCollection services, Action<ApiOptions> getOptions)
    {
        var options = new ApiOptions();
        getOptions(options);
        services.AddSingleton(options);
        services.AddTransient<Cqrs.ApiCommand, ApiCommand>();
        services.AddTransient<Cqrs.ApiQuery, ApiQuery>();
        return services;
    }
}
