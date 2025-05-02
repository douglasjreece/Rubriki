using Rubriki.Api;
using Rubriki.Authentication;
using System.Net.Http.Json;
using System.Text.Json;

namespace Rubriki.App.Authentication;

public class AppAuthenticationService(AppAuthenticationService.Options options) : ISecretCodeAuthenticationService
{
    public class Options
    {
        public Uri ApiEndpoint { get; set; } = default!;
        public string AuthDataFilePath { get; set; } = default!;
    }

    public async Task<AuthenticationResult> GetSignedInState()
    {
        try
        {
            var jsonText = await File.ReadAllTextAsync(options.AuthDataFilePath);
            var authData = JsonSerializer.Deserialize<AuthenticationResult>(jsonText);
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
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
            if (result is null)
            {
                throw new Exception("Failed to deserialize authentication result.");
            }

            if (persist)
            {
                var jsonText = JsonSerializer.Serialize(result);
                await File.WriteAllTextAsync(options.AuthDataFilePath, jsonText);
            }

            return result;
        }
        else
        {
            return AuthenticationResult.Empty;
        }
    }

    public async Task SignOut()
    {
        await Task.Run(() =>
        {
            if (File.Exists(options.AuthDataFilePath))
            {
                File.Delete(options.AuthDataFilePath);
            }
        });
    }

}
