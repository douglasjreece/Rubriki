using Rubriki.Dto;

namespace Rubriki.Cqrs;

public abstract class ApiCommand
{
    public abstract Task SubmitScore(ScoreEntry entry);
}
