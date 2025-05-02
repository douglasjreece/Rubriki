namespace Rubriki.Website.CookieCqrs;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCookieCqrs(this IServiceCollection services, Action<CookieCommand.Options>? getOptions)
    {
        var options = new CookieCommand.Options();
        getOptions?.Invoke(options);
        services.AddSingleton(options);
        services.AddScoped<CookieCommand>();
        services.AddScoped<CookieQuery>();
        return services;
    }

    public static IServiceCollection AddCookieCqrs(this IServiceCollection services)
    {
        return AddCookieCqrs(services, null);
    }
}
