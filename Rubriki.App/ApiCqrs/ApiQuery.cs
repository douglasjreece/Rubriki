using Rubriki.Api;
using Rubriki.Authentication;
using Rubriki.Dto;
using System.Net.Http.Json;

namespace Rubriki.App.ApiCqrs;

public class ApiQuery(ISecretCodeAuthenticationService authenticationService, ApiOptions options) : Cqrs.ApiQuery
{
    public override async Task<SeedData> GetSeedData()
    {
        var authState = await authenticationService.GetSignedInState();

        using var client = new HttpClient();
        var url = new UriBuilder(options.ApiEndpoint) { Path = $"{ApiConst.AppPath}/{ApiConst.SeedDataResource}" }.ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, url)
        {
            Headers =
                {
                    { "Token", authState?.Token }
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

        return seedData;
    }
}
