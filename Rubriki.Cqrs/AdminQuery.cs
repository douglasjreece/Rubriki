using Microsoft.EntityFrameworkCore;
using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class AdminQuery(Repository.ApplicationDbContext db)
{
    public async Task<List<ContestantTotalScore>> GetResults()
    {
        return await db.Scores
            .Include(x => x.Contestant)
            .GroupBy(x => x.Contestant)
            .Select(x => new ContestantTotalScore(new(x.Key!.Id, x.Key.Name), x.Sum(y => y.Value)))
            .ToListAsync();
    }
}
