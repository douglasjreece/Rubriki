using Rubriki.Api;
using Rubriki.Authentication;
using Rubriki.Cqrs;
using System.Net.Http.Json;
using System.Security.Authentication;

namespace Rubriki.App.Authentication;

public class AppAuthenticationService(StorageQuery storageQuery, StorageCommand storageCommand, AppAuthenticationService.Options options) : ISecretCodeAuthenticationService
{
    public class Options
    {
        public Uri ApiEndpoint { get; set; } = default!;
        public string AuthDataFileName { get; set; } = default!;
    }

    public async Task<AuthenticationResult> GetSignedInState()
    {
        try
        {
            var authData = await storageQuery.GetOrDefault<AuthenticationResult>(options.AuthDataFileName);
            return authData ?? AuthenticationResult.Empty;
        }
        catch
        {
            return AuthenticationResult.Empty;
        }
    }

    public Task<AuthenticationResult> GetStateForToken(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthenticationResult> SignIn(string secretCode, bool persist)
    {
        var url = new UriBuilder(options.ApiEndpoint) { Path = $"{ApiConst.AppPath}/{ApiConst.AuthenticationResource}" }.ToString();
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new AuthenticationRequest(secretCode))
        };

        var client = new HttpClient();
        var response = await client.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new AuthenticationException("Invalid secret code.");
        }
        response.EnsureSuccessStatusCode(); // throw exception for any other non-success status code

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        if (result is null)
        {
            throw new InvalidOperationException("Failed to deserialize authentication result.");
        }

        if (persist)
        {
            await storageCommand.Store(options.AuthDataFileName, result);
        }

        return result;
    }

    public async Task SignOut()
    {
        await storageCommand.Remove(options.AuthDataFileName);
    }

}
