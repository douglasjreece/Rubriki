namespace Rubriki.Interfaces;

public interface IScoreQuery
{
    Task<List<CriteriaScore>> GetContestantCriteriaScores(int contestantId, int categoryId);
    Task<List<CriteriaScore>> GetContestantCriteriaScores(int contestantId);
    Task<List<ContestantTotalScore>> GetContestantTotals();
    Task<List<ContestantTotalScore>> GetContestantTotalsForCategory(int categoryId);
    Task<int> GetScoredContestantCount();
    Task<List<ScoreEntry>> GetScores();
}
