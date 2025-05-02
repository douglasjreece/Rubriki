using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rubriki.App.ApiCqrs;
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
        const string dbFileName = "data.db";
		const string authStateFileName = "authstate.json";
        const string apiEndpoint = "https://localhost:7254";
        //const string apiEndpoint = "http://10.0.2.2:5180"; // https://localhost:7254";
        //const string apiEndpoint = "https://www.songshowplus.com";

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
        services.AddCqrs(options =>
        {
            options.StorageOptions.AppDataDirectory = appDataDirectory;
        });
        services.AddApiCqrs(options =>
        {
            options.ApiEndpoint = new Uri(apiEndpoint);
        });
		services.AddAppUseCases();

        services.AddCascadingAuthenticationState();
        services.AddAuthorizationCore();

        services.AddSecretCodeAuthentication();
        services.AddAppAuthentication(options =>
        {
            options.ApiEndpoint = new Uri(apiEndpoint);
            options.AuthDataFileName = authStateFileName;
        });

        services.AddSharedComponents();

        return builder.Build();
	}
}
