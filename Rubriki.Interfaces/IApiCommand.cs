namespace Rubriki.Interfaces;

public interface IApiCommand
{
    Task SubmitScore(ScoreEntry entry);
}
