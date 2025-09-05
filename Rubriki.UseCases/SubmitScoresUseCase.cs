using Rubriki.Interfaces;

namespace Rubriki.UseCases;

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
