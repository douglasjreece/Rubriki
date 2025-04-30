using Microsoft.EntityFrameworkCore;
using Rubriki.Repository;

namespace Rubriki.Cqrs;

public class AdminCommand(ApplicationDbContext db)
{
    public async Task SeedData(Dto.SeedData seedData)
    {
        var categories = new List<Category>();
        var criteria = new List<Criteria>();
        var contestants = new List<Contestant>();
        var judges = new List<Judge>();
        var levels = new List<Level>();
        foreach (var (categoryName, criteriaNames) in seedData.CategoryAndCriteria)
        {
            var category = new Category { Name = categoryName };
            categories.Add(category);
            foreach (var criteriaName in criteriaNames)
            {
                criteria.Add(new Criteria { Name = criteriaName, Category = category });
            }
        }
        foreach (var contestantName in seedData.ContestantNames)
        {
            contestants.Add(new Contestant { Name = contestantName });
        }
        foreach (var judgeName in seedData.JudgeNames)
        {
            judges.Add(new Judge { Name = judgeName });
        }
        for (var i = 0; i < seedData.Levels.Count; i++)
        {
            levels.Add(new Level { Description = seedData.Levels[i], Score = i + 1 });
        }
        await db.Categories.AddRangeAsync(categories);
        await db.Criteria.AddRangeAsync(criteria);
        await db.Contestants.AddRangeAsync(contestants);
        await db.Judges.AddRangeAsync(judges);
        await db.Levels.AddRangeAsync(levels);
        await db.SaveChangesAsync();
    }

    public async Task ClearData()
    {
        var contestants = await db.Contestants.ToListAsync();
        var judges = await db.Judges.ToListAsync();
        var categories = await db.Categories.ToListAsync();
        var criteria = await db.Criteria.ToListAsync();
        var scores = await db.Scores.ToListAsync();
        var levels = await db.Levels.ToListAsync();
        db.Scores.RemoveRange(scores);
        db.Contestants.RemoveRange(contestants);
        db.Judges.RemoveRange(judges);
        db.Categories.RemoveRange(categories);
        db.Criteria.RemoveRange(criteria);
        db.Levels.RemoveRange(levels);
        await db.SaveChangesAsync();
    }

    public async Task ClearScores()
    {
        var scores = await db.Scores.ToListAsync();
        db.Scores.RemoveRange(scores);
        await db.SaveChangesAsync();
    }
}
