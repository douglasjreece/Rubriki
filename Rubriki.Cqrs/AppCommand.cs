using Microsoft.EntityFrameworkCore;
using Rubriki.Repository;

namespace Rubriki.Cqrs;

public class AppCommand(ApplicationDbContext db)
{
    public async Task ClearScores()
    {
        var scores = await db.Scores.ToListAsync();
        db.Scores.RemoveRange(scores);
        await db.SaveChangesAsync();
    }
}
