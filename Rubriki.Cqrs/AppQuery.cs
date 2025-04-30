using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class AppQuery(Repository.ApplicationDbContext db)
{
    public async Task<List<ScoreEntry>> GetScores()
    {
        return await db.Scores
            .Include(x => x.Contestant)
            .Include(x => x.Judge)
            .Include(x => x.Level)
            .Include(x => x.Criteria)
            .ThenInclude(x => x.Category)
            .Select(x => new ScoreEntry(
                new Contestant(x.Contestant!.Id, x.Contestant.Name),
                new Judge(x.Judge!.Id, x.Judge.Name),
                new Criteria(new Category(x.Criteria!.Category!.Id, x.Criteria.Category.Name), x.Criteria.Id, x.Criteria.Name),
                new Level(x.Level!.Id, x.Level.Description, x.Level.Score), 
               x.Comment))
            .ToListAsync();
    }
}
