using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Rubriki.App.Components;
using Rubriki.Cqrs;
using Rubriki.Repository;
using Rubriki.SharedComponents;
using Rubriki.UseCases;

namespace Rubriki.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
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

		const string dbFileName = "rubriki.db";
        var dbFilePath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={dbFilePath}"));

		services.AddModels();
		services.AddAppCqrs();
		services.AddAppUseCases(options =>
		{
			options.ApiEndpoint = new("https://localhost:7254");
		});

		services.AddSharedComponents();

        return builder.Build();
	}
}
