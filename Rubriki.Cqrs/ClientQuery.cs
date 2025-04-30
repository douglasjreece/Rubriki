using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class ClientQuery(Repository.ApplicationDbContext db) : CqrsQuery
{
    public async Task<List<Contestant>> GetContestants()
    {
        return await db.Contestants.Select(x => Map(x)).ToListAsync();
    }

    public async Task<List<Judge>> GetJudges()
    {
        return await db.Judges.Select(x => Map(x)).ToListAsync();
    }

    public async Task<List<Category>> GetCategories()
    {
        return await db.Categories.Select(x => Map(x)).ToListAsync();
    }

    public async Task<List<Criteria>> GetCriteria()
    {
        return await db.Criteria
            .Include(x => x.Category)
            .Select(x => Map(x))
            .ToListAsync();
    }

    public async Task<List<Criteria>> GetCriteria(int categoryId)
    {
        return await db.Criteria
            .Include(x => x.Category)
            .Where(x => x.Category!.Id == categoryId)
            .Select(x => Map(x))
            .ToListAsync();
    }

    public async Task<List<Level>> GetLevels()
    {
        return await db.Levels
            .Select(x => Map(x))
            .ToListAsync();
    }

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

    public async Task<Contestant> GetContestant(int contestantId)
    {
        return await db.Contestants
            .Where(x => x.Id == contestantId)
            .Select(x => Map(x))
            .FirstAsync();
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
