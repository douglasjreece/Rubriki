using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class ClientQuery(Repository.ApplicationDbContext db)
{
    public async Task<List<Contestant>> GetContestants()
    {
        return [..(await db.Contestants.ToListAsync()).Select(x => new Contestant(x.Id, x.Name))];
    }

    public async Task<List<Judge>> GetJudges()
    {
        return [.. (await db.Judges.ToListAsync()).Select(x => new Judge(x.Id, x.Name))];
    }

    public async Task<List<Category>> GetCategories()
    {
        return await db.Categories
            .Select(x => new Category(x.Id, x.Name))
            .ToListAsync();
    }

    public async Task<List<Criteria>> GetCriteria()
    {
        return await db.Criteria
            .Include(x => x.Category)
            .Select(x => new Criteria(new(x.Category!.Id, x.Category.Name), x.Id, x.Name))
            .ToListAsync();
    }

    public async Task<List<Criteria>> GetCriteria(int categoryId)
    {
        return await db.Criteria
            .Include(x => x.Category)
            .Where(x => x.Category!.Id == categoryId)
            .Select(x => new Criteria(new(x.Category!.Id, x.Category.Name), x.Id, x.Name))
            .ToListAsync();
    }

    public async Task<List<Level>> GetLevels()
    {
        return await db.Levels
            .Select(x => new Level(x.Id, x.Description, x.Score))
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
            .Select(x => new CriteriaScore(
                new Criteria(
                    new Category(x.Criteria!.Category!.Id, x.Criteria!.Category!.Name), 
                    x.Criteria!.Id, 
                    x.Criteria.Name),
                new Judge(x.Judge!.Id, x.Judge.Name),
                new Level(x.Level!.Id, x.Level.Description, x.Level.Score),
                x.Comment))
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
            .Select(x => new CriteriaScore(
                new Criteria(
                    new Category(x.Criteria!.Category!.Id, x.Criteria!.Category!.Name),
                    x.Criteria!.Id,
                    x.Criteria.Name),
                new Judge(x.Judge!.Id, x.Judge.Name),
                new Level(x.Level!.Id, x.Level.Description, x.Level.Score),
                x.Comment))
            .ToListAsync();
    }

    public async Task<Contestant> GetContestant(int contestantId)
    {
        return await db.Contestants
            .Where(x => x.Id == contestantId)
            .Select(x => new Contestant(x.Id, x.Name))
            .FirstAsync();
    }
}
