using Microsoft.EntityFrameworkCore;

namespace Rubriki.Cqrs;

public class ClientCommand(Repository.ApplicationDbContext db)
{
#if false
    public async Task<bool> AddContestantAsync(string name)
    {
        var contestant = new Repository.Contestant { Name = name };
        db.Contestants.Add(contestant);
        return await db.SaveChangesAsync() > 0;
    }
#endif

    public async Task SetScore(int contestantId, int judgeId, int criteriaId, int score, string comment)
    {
        var contestant = await db.Contestants.FindAsync(contestantId);
        var judge = await db.Judges.FindAsync(judgeId);
        var criteria = await db.Criteria.FindAsync(criteriaId);
        if (contestant == null || judge == null || criteria == null)
            throw new ArgumentException("Invalid contestant, judge or criteria ID");
        var existingScore = await db.Scores
            .FirstOrDefaultAsync(x => x.Contestant!.Id == contestantId && x.Judge!.Id == judgeId && x.Criteria!.Id == criteriaId);
        if (existingScore != null)
        {
            existingScore.Value = score;
            existingScore.Comment = comment;
        }
        else
        {
            var newScore = new Repository.Score 
            { 
                Contestant = contestant, 
                Judge = judge, 
                Criteria = criteria, 
                Value = score,
                Comment = comment
            };
            db.Scores.Add(newScore);
        }
        await db.SaveChangesAsync();
    }
}
