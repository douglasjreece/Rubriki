using Microsoft.EntityFrameworkCore;

namespace Rubriki.Cqrs;

public class ScoreQuery(Repository.ApplicationDbContext db) : IScoreQuery
{
    public async Task<List<CriteriaScore>> GetContestantCategoryScores(int contestantId, int categoryId)
    {
        return await db.Scores
            .Include(x => x.Criteria)
            .Include(x => x.Judge)
            .Include(x => x.Level)
            .Include(x => x.Criteria!.Category)
            .Where(x => x.Contestant!.Id == contestantId && x.Criteria!.Category!.Id == categoryId)
            .Select(x => new CriteriaScore(Map.ToDto(x.Criteria), Map.ToDto(x.Judge), Map.ToDto(x.Level), x.Comment))
            .ToListAsync();
    }

    public async Task<List<CriteriaScore>> GetContestantCategoryScores(int contestantId)
    {
        return await db.Scores
            .Include(x => x.Criteria)
            .Include(x => x.Judge)
            .Include(x => x.Level)
            .Include(x => x.Criteria!.Category)
            .Where(x => x.Contestant!.Id == contestantId)
            .Select(x => 
                new CriteriaScore(
                    Map.ToDto(x.Criteria),
                    Map.ToDto(x.Judge),
                    Map.ToDto(x.Level),
                    x.Comment)
                )
            .ToListAsync();
    }

    public async Task<List<ContestantTotalScore>> GetContestantTotals()
    {
        return await db.Scores
            .Include(x => x.Contestant)
            .Include(x => x.Level)
            .GroupBy(x => x.Contestant)
            .Select(x => new ContestantTotalScore(Map.ToDto(x.Key), x.Sum(y => y.Level!.Score)))
            .ToListAsync();
    }

    public async Task<List<ScoreEntry>> GetScores()
    {
        return await db.Scores
            .Include(x => x.Contestant)
            .Include(x => x.Judge)
            .Include(x => x.Level)
            .Include(x => x.Criteria)
            .ThenInclude(x => x.Category)
            .Select(x =>
                new ScoreEntry(
                    Map.ToDto(x.Contestant),
                    Map.ToDto(x.Judge),
                    Map.ToDto(x.Criteria),
                    Map.ToDto(x.Level),
                    x.Comment)
                )
            .ToListAsync();
    }

    public async Task<int> GetScoredContestantCount()
    {
        return await db.Scores
            .Select(x => x.Contestant!.Id)
            .Distinct()
            .CountAsync();
    }
}
