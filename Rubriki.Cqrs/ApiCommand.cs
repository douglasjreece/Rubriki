namespace Rubriki.Cqrs;

public abstract class ApiCommand : IApiCommand
{
    public abstract Task SubmitScore(ScoreEntry entry);
}
