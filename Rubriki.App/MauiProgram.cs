using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rubriki.App.Authentication;
using Rubriki.App.Components;
using Rubriki.Authentication;
using Rubriki.Cqrs;
using Rubriki.Repository;
using Rubriki.SharedComponents;
using Rubriki.UseCases;

namespace Rubriki.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        string appDataDirectory = FileSystem.AppDataDirectory;
        const string dbFileName = "rubriki.db";
		const string authStateFileName = "authstate.json";
        const string apiEndpoint = "https://localhost:7254";

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();

#endif
        var services = builder.Services;

        var dbFilePath = Path.Combine(appDataDirectory, dbFileName);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={dbFilePath}"));

		services.AddModels();
		services.AddAppCqrs();
		services.AddAppUseCases(options =>
		{
			options.ApiEndpoint = new Uri(apiEndpoint);
		});

        services.AddCascadingAuthenticationState();
        services.AddAuthorizationCore();

        services.AddSecretCodeAuthentication();
        services.AddAppAuthentication(options =>
        {
            options.ApiEndpoint = new Uri(apiEndpoint);
            options.AuthDataFilePath = Path.Combine(appDataDirectory, authStateFileName);
        });

        services.AddSharedComponents();

        return builder.Build();
	}
}
