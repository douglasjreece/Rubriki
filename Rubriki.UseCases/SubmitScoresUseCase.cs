using Rubriki.Cqrs;

namespace Rubriki.UseCases;

public class SubmitScoresUseCase(AppQuery query, AppCommand command, ApiCommand apiCommand)
{
    public async Task Invoke()
    {
        var scores = await query.GetScores();
        foreach (var score in scores)
        {
            await apiCommand.SubmitScore(score);
        }

        await command.ClearScores();
    }
}
