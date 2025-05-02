using Microsoft.Extensions.DependencyInjection;

namespace Rubriki.UseCases;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAdminUseCases(this IServiceCollection services)
    {
        services.AddScoped<SeedDatabaseUseCase>();
        return services;
    }

    public static IServiceCollection AddClientUseCases(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddAppUseCases(this IServiceCollection services)
    {
        services.AddScoped<InitializeAppDatabaseUseCase>();
        services.AddScoped<SubmitScoresUseCase>();
        return services;
    }
}
