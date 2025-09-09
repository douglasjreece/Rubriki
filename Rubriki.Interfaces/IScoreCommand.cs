namespace Rubriki.Interfaces;

public interface IScoreCommand
{
    Task ClearScores();
    Task SetScore(int contestantId, int judgeId, int criteriaId, int levelId, string comment);
}
