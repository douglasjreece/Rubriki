using Microsoft.EntityFrameworkCore;
using Rubriki.Repository;

namespace Rubriki.Cqrs;

public class SetupCommand(ApplicationDbContext db) : ISetupCommand
{
    public async Task EnsureDatabaseIsCreated()
    {
        await db.Database.EnsureCreatedAsync();
    }

    public async Task Seed(SeedData seedData)
    {
        var categories = new List<Entities.Category>();
        var criteria = new List<Entities.Criteria>();
        var contestants = new List<Entities.Contestant>();
        var judges = new List<Entities.Judge>();
        var levels = new List<Entities.Level>();
        foreach (var (categoryName, criteriaNames) in seedData.CategoryAndCriteria)
        {
            var category = new Entities.Category { Id = categories.Count + 1, Name = categoryName };
            categories.Add(category);
            foreach (var criteriaName in criteriaNames)
            {
                criteria.Add(new Entities.Criteria { Id = criteria.Count + 1, Name = criteriaName, Category = category });
            }
        }
        foreach (var contestantName in seedData.ContestantNames)
        {
            contestants.Add(new Entities.Contestant { Id = contestants.Count + 1, Name = contestantName });
        }
        foreach (var judgeName in seedData.JudgeNames)
        {
            judges.Add(new Entities.Judge { Id = judges.Count + 1, Name = judgeName });
        }
        for (var i = 0; i < seedData.Levels.Count; i++)
        {
            levels.Add(new Entities.Level { Id = levels.Count + 1, Description = seedData.Levels[i], Score = i + 1 });
        }
        await db.Categories.AddRangeAsync(categories);
        await db.Criteria.AddRangeAsync(criteria);
        await db.Contestants.AddRangeAsync(contestants);
        await db.Judges.AddRangeAsync(judges);
        await db.Levels.AddRangeAsync(levels);
        await db.SaveChangesAsync();
    }

    public async Task Clear()
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
}
