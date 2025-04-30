using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class AppQuery(Repository.ApplicationDbContext db) : CqrsQuery
{
    public async Task<int> GetScoredContestantCount()
    {
        return await db.Scores
            .Select(x => x.Contestant!.Id)
            .Distinct()
            .CountAsync();
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
                    Map(x.Contestant),
                    Map(x.Judge),
                    Map(x.Criteria),
                    Map(x.Level), 
                    x.Comment)
                )
            .ToListAsync();
    }
}
