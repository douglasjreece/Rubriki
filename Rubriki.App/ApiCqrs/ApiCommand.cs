using Rubriki.Api;
using Rubriki.Authentication;
using Rubriki.Dto;
using System.Net.Http.Json;

namespace Rubriki.App.ApiCqrs;

public class ApiCommand(ISecretCodeAuthenticationService authenticationService, ApiOptions options) : Cqrs.ApiCommand
{
    public override async Task SubmitScore(ScoreEntry entry)
    {
        var authState = await authenticationService.GetSignedInState();

        ScoreSubmission submission = new(
            entry.Contestant.Id,
            entry.Judge.Id,
            entry.Criteria.Id,
            entry.Level.Id,
            entry.Comment
            );
        var url = new UriBuilder(options.ApiEndpoint) { Path = $"{ApiConst.AppPath}/{ApiConst.ScoreResource}/{entry.Contestant.Id}" }.ToString();
        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(submission),
            Headers =
                {
                    { ApiConst.TokenHeader, authState?.Token }
                }
        };
        using var client = new HttpClient();
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to submit score for contestant {entry.Contestant.Id}. Code: {response.StatusCode}.", null, response.StatusCode);
        }
    }

}
