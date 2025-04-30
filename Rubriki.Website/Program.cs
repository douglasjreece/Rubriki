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

        var dbFilePath = configuration.GetValue<string>("Database:FilePath");

        services.AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlite($"Data Source={dbFilePath}"));

        services.AddCqrs();
        services.AddAdminUseCases();
        services.AddClientUseCases();

        services.AddSharedComponents();
        services.AddWebsiteModels();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

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
}
