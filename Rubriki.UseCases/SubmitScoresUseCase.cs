using Rubriki.Cqrs;

namespace Rubriki.UseCases;

public class SubmitScoresUseCase(ScoreQuery scoreQuery, ScoreCommand scoreCommand, ApiCommand apiCommand)
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
