using Rubriki.Cqrs;
using Rubriki.Dto;
using System.Net.Http.Json;

namespace Rubriki.UseCases;

public class SubmitScoresUseCase(AppQuery query, AppCommand command, AppUseCaseOptions options)
{
    public async Task Invoke(string secretCode)
    {
        var client = new HttpClient();

        var scores = await query.GetScores();

        foreach (var score in scores)
        {
            ScoreSubmission submission = new(
                score.Contestant.Id,
                score.Judge.Id,
                score.Criteria.Id,
                score.Level.Id,
                score.Comment
                );
            var url = new UriBuilder(options.ApiEndpoint) { Path = $"/api/app/submit-score/{score.Contestant.Id}" }.ToString();
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = JsonContent.Create(submission),
                Headers =
                {
                    { "SecretCode", secretCode }
                }
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to submit score for contestant {score.Contestant.Id}. Code: {response.StatusCode}.");
            }
        }

        await command.ClearScores();
    }

}
