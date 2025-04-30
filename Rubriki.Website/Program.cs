using Microsoft.EntityFrameworkCore;
using Rubriki.Cqrs;
using Rubriki.Repository;
using Rubriki.SharedComponents;
using Rubriki.UseCases;
using Rubriki.Website.Components;

namespace Rubriki.Website;

public class Program
{
    public static void Main(string[] args)
    {
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

        var dbFilePath = configuration.GetValue<string>("Database:FilePath") ?? throw new InvalidOperationException("Database:FilePath is not set.");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={dbFilePath}"));

        services.AddControllers();

        services.AddWebsiteCqrs();
        services.AddAdminUseCases();
        services.AddClientUseCases();

        services.AddSharedComponents();
        services.AddWebsiteModels();

        services.AddSingleton(seedData);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAntiforgery();
        app.MapControllers();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();


        {
            var serviceProvider = app.Services;
            using var scope = serviceProvider.CreateScope();
            var scopeSservices = scope.ServiceProvider;
            var dbContext = scopeSservices.GetRequiredService<ApplicationDbContext>();
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
