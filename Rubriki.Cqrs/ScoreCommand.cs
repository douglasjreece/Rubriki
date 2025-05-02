using Microsoft.EntityFrameworkCore;

namespace Rubriki.Cqrs;

public class ScoreCommand(Repository.ApplicationDbContext db)
{
    public async Task SetScore(int contestantId, int judgeId, int criteriaId, int levelId, string comment)
    {
        var contestant = await db.Contestants.FindAsync(contestantId) ?? throw new ArgumentException($"Contestant not found: {contestantId}.");
        var judge = await db.Judges.FindAsync(judgeId) ?? throw new ArgumentException($"Judge not found: {judgeId}.");
        var criteria = await db.Criteria.FindAsync(criteriaId) ?? throw new ArgumentException($"Criteria not found: {criteriaId}.");
        var level = await db.Levels.FindAsync(levelId) ?? throw new ArgumentException($"Level not found: {levelId}.");

        var existingScore = await db.Scores.FirstOrDefaultAsync(x => x.Contestant!.Id == contestantId && x.Criteria!.Id == criteriaId);
        if (existingScore != null)
        {
            existingScore.Level = level;
            existingScore.Comment = comment;
        }
        else
        {
            var newScore = new Repository.Score 
            { 
                Contestant = contestant, 
                Judge = judge, 
                Criteria = criteria, 
                Level = level,
                Comment = comment
            };
            db.Scores.Add(newScore);
        }

        await db.SaveChangesAsync();
    }
}
