using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class ScoreQuery(Repository.ApplicationDbContext db) : CqrsQuery
{
    public async Task<List<CriteriaScore>> GetContestantCategoryScores(int contestantId, int categoryId)
    {
        return await db.Scores
            .Include(x => x.Criteria)
            .Include(x => x.Judge)
            .Include(x => x.Level)
            .Include(x => x.Criteria!.Category)
            .Where(x => x.Contestant!.Id == contestantId && x.Criteria!.Category!.Id == categoryId)
            .Select(x => 
                new CriteriaScore(
                    Map(x.Criteria),
                    Map(x.Judge),
                    Map(x.Level),
                    x.Comment)
                )
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
                    Map(x.Criteria),
                    Map(x.Judge),
                    Map(x.Level),
                    x.Comment)
                )
            .ToListAsync();
    }

    public async Task<List<ContestantTotalScore>> GetResults()
    {
        return await db.Scores
            .Include(x => x.Contestant)
            .Include(x => x.Level)
            .GroupBy(x => x.Contestant)
            .Select(x => new ContestantTotalScore(Map(x.Key), x.Sum(y => y.Level!.Score)))
            .ToListAsync();
    }
}
