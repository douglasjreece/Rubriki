using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class SetupQuery(Repository.ApplicationDbContext db) : CqrsQuery
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

    public async Task<Contestant> GetContestant(int contestantId)
    {
        return await db.Contestants
            .Where(x => x.Id == contestantId)
            .Select(x => Map(x))
            .FirstAsync();
    }
}
