using Microsoft.EntityFrameworkCore;
using Rubriki.Cqrs;
using Rubriki.Repository;
using Rubriki.SharedComponents;
using Rubriki.UseCases;
using Rubriki.Authentication;
using Rubriki.Website.Components;
using Rubriki.Website.CookieCqrs;

namespace Rubriki.Website;

public class Program
{
    public static void Main(string[] args)
    {
        string appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Rubriki");
        const string dbFileName = "data.db";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddCircuitOptions(options =>
            {
                options.DetailedErrors = true;
                options.DisconnectedCircuitMaxRetained = 0;
                options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromSeconds(0);
            });

        var services = builder.Services;
        var configuration = builder.Configuration;

        //var dbFilePath = configuration.GetValue<string>("Database:FilePath") ?? throw new InvalidOperationException("Database:FilePath is not set.");
        var dbFilePath = Path.Combine(appDataDirectory, dbFileName);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={dbFilePath}"));

        services.AddControllers();

        services.AddCqrs(options =>
        {
            options.StorageOptions.AppDataDirectory = appDataDirectory;
        });
        services.AddAdminUseCases();
        services.AddClientUseCases();

        services.AddSharedComponents();
        services.AddWebsiteModels();

        services.AddSingleton(seedData);

        services.AddAuthentication();
        services.AddCascadingAuthenticationState();

        services.AddSecretCodeAuthentication();
        services.AddCookieCqrs();
        services.AddServerAuthentication(options =>
        {
            options.JudgeCode = configuration.GetValue<string>("Auth:JudgeCode") ?? throw new InvalidOperationException("JudgeCode is not set.");
            options.AdminCode = configuration.GetValue<string>("Auth:AdminCode") ?? throw new InvalidOperationException("AdminCode is not set.");
            options.AesEncryptionKey = new Guid("{131ADCF2-7243-4179-AECF-FC3228A94946}");
            options.AesEncryptionIV = new Guid("{A0E4D1F2-3C7B-4F5A-8E6C-9D5B2A0D3E8F}");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

#if !DEBUG // Disable HTTPS redirection in debug mode otherwise emulator cannot connect to REST API
        app.UseHttpsRedirection();
#endif
        app.UseRouting();
        app.UseAntiforgery();
        app.MapControllers();

        //app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();


        {
            var serviceProvider = app.Services;
            using var scope = serviceProvider.CreateScope();
            var scopeSservices = scope.ServiceProvider;
            var dbContext = scopeSservices.GetRequiredService<ApplicationDbContext>();
            Directory.CreateDirectory(appDataDirectory);
            dbContext.Database.EnsureCreated();
        }

        app.Run();
    }

    private static readonly Dto.SeedData seedData = new Dto.SeedData
    {
        CategoryAndCriteria =
        {
            {"Robot Design", ["Mechanical", "Programming", "Innovation" ]},
            {"Project", ["Research", "Solution", "Presentation"]},
            {"Core Values", [ "Inspiration", "Teamwork", "Professionalism"]},
        },
        ContestantNames =
        [
            "Astro Bots",
            "Bot Heads",
            "Code Commets",
            "Galactic Gearheads",
            "Hydro Hackers",
            "Mech Masters",
            "Null Terminators",
            "Robo Rangers",
            "Salty Circuits",
            "Wattage Warriors",
        ],
        JudgeNames =
        [
            "Alice Anderson",
            "Brian Bennett",
            "Catherine Carter",
            "Daniel Davis"
        ],
        Levels =
        [
            "Beginner",
            "Developing",
            "Accomplished",
            "Exemplary"
        ]
    };
}
