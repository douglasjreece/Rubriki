using Microsoft.EntityFrameworkCore;

namespace Rubriki.Cqrs;

public class SetupQuery(Repository.ApplicationDbContext db) : CqrsQuery, ISetupQuery
{
    public async Task<List<Dto.Contestant>> GetContestants()
    {
        return await db.Contestants.Select(x => ToDto(x)).ToListAsync();
    }

    public async Task<List<Dto.Judge>> GetJudges()
    {
        return await db.Judges.Select(x => ToDto(x)).ToListAsync();
    }

    public async Task<List<Dto.Category>> GetCategories()
    {
        return await db.Categories.Select(x => ToDto(x)).ToListAsync();
    }

    public async Task<List<Dto.Criteria>> GetCriteria()
    {
        return await db.Criteria
            .Include(x => x.Category)
            .Select(x => ToDto(x))
            .ToListAsync();
    }

    public async Task<List<Dto.Criteria>> GetCriteria(int categoryId)
    {
        return await db.Criteria
            .Include(x => x.Category)
            .Where(x => x.Category!.Id == categoryId)
            .Select(x => ToDto(x))
            .ToListAsync();
    }

    public async Task<List<Dto.Level>> GetLevels()
    {
        return await db.Levels
            .Select(x => ToDto(x))
            .ToListAsync();
    }

    public async Task<Dto.Contestant> GetContestant(int contestantId)
    {
        return await db.Contestants
            .Where(x => x.Id == contestantId)
            .Select(x => ToDto(x))
            .FirstAsync();
    }
}
