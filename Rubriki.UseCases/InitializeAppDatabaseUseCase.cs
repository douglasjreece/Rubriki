using Rubriki.Cqrs;
using Rubriki.Dto;
using System.Net.Http.Json;

namespace Rubriki.UseCases;

public class InitializeAppDatabaseUseCase(SetupCommand command, AppUseCaseOptions options)
{
    public async Task Invoke(string authToken)
    {
        var client = new HttpClient();
        var url = new UriBuilder(options.ApiEndpoint) { Path = "/api/app/seed-data" }.ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, url)
        {
            Headers =
                {
                    { "Token", authToken }
                }
        };

        var response = await client.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get seed data. Code: {response.StatusCode}");
        }

        var seedData = await response.Content.ReadFromJsonAsync<SeedData>();
        if (seedData is null)
        {
            throw new Exception($"Failed to deserialize seed data.");
        }

        await command.EnsureDatabaseIsCreated();
        await command.Clear();
        await command.Seed(seedData);
    }
}
