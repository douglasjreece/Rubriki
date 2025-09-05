namespace Rubriki.Interfaces;

public interface IScoreQuery
{
    Task<List<CriteriaScore>> GetContestantCategoryScores(int contestantId, int categoryId);
    Task<List<CriteriaScore>> GetContestantCategoryScores(int contestantId);
    Task<List<ContestantTotalScore>> GetContestantTotals();
    Task<int> GetScoredContestantCount();
    Task<List<ScoreEntry>> GetScores();
}
