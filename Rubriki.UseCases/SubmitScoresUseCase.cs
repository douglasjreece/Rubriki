using Rubriki.Interfaces;

namespace Rubriki.UseCases;

/// <summary>
/// Handles the submission of local device scores to the shared repository.
/// </summary>
/// <remarks>This use case retrieves scores from the provided <see cref="IScoreQuery"/>, submits each score to the
/// external API using the <see cref="IApiCommand"/>,  and then clears the scores from the local storage using the <see
/// cref="IScoreCommand"/>.  Ensure that the dependencies are properly configured and injected before invoking this use
/// case.</remarks>
/// <param name="scoreQuery"></param>
/// <param name="scoreCommand"></param>
/// <param name="apiCommand"></param>
public class SubmitScoresUseCase(IScoreQuery scoreQuery, IScoreCommand scoreCommand, IApiCommand apiCommand)
{
    public async Task Invoke()
    {
        var scores = await scoreQuery.GetScores();
        foreach (var score in scores)
        {
            await apiCommand.SubmitScore(score);
        }

        await scoreCommand.ClearScores();
    }
}
