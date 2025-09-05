using Microsoft.EntityFrameworkCore;

namespace Rubriki.Infrastructure.Tests
{
    [TestClass]
    public sealed class ClientTests
    {
        [TestMethod]
        public async Task ScoreTwoCriteria()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Repository.ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var context = new Repository.ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var adminCommand = new Cqrs.SetupCommand(context);
            var clientCommand = new Cqrs.ScoreCommand(context);
            var setupQuery = new Cqrs.SetupQuery(context);
            var scoreQuery = new Cqrs.ScoreQuery(context);
            var seed = new Dto.SeedData
            {
                CategoryAndCriteria =
                {
                    { "Category1", ["Criteria1", "Criteria2" ] },
                    { "Category2", [ "Criteria3" ] }
                },
                ContestantNames = ["Contestant1"],
                JudgeNames = ["Judge1", "Judge2"],
                Levels = ["Level1", "Level2", "Level3"]
            };

            // Act
            await adminCommand.Seed(seed);
            var contestants = await setupQuery.GetContestants();
            var contestant = contestants.First();
            var judges = await setupQuery.GetJudges();
            var judge = judges.First();
            var categories = await setupQuery.GetCategories();
            var category = categories.First();
            var criterias = await setupQuery.GetCriteria(category.Id);
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[0].Id, 1, "");
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[1].Id, 2, "");
            var results = await scoreQuery.GetContestantTotals();
            var contestantResult = results.First();

            // Assert
            Assert.AreEqual(3, contestantResult.Score);
        }

        [TestMethod]
        public async Task ChangeScore()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Repository.ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var context = new Repository.ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var adminCommand = new Cqrs.SetupCommand(context);
            var clientCommand = new Cqrs.ScoreCommand(context);
            var setupQuery = new Cqrs.SetupQuery(context);
            var scoreQuery = new Cqrs.ScoreQuery(context);
            var seed = new Dto.SeedData
            {
                CategoryAndCriteria =
                {
                    { "Category1", ["Criteria1", "Criteria2" ]},
                    { "Category2", [ "Criteria3" ] }
                },
                ContestantNames = ["Contestant1"],
                JudgeNames = ["Judge1", "Judge2"],
                Levels = ["Level1", "Level2", "Level3"]
            };

            // Act
            await adminCommand.Seed(seed);
            var contestants = await setupQuery.GetContestants();
            var contestant = contestants.First();
            var judges = await setupQuery.GetJudges();
            var judge = judges.First();
            var categories = await setupQuery.GetCategories();
            var category = categories.First();
            var criterias = await setupQuery.GetCriteria(category.Id);
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[0].Id, 1, "");
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[1].Id, 2, "");
            var firstResults = await scoreQuery.GetContestantTotals();
            var firstContestantResult = firstResults.First();
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[1].Id, 3, "");
            var secondResults = await scoreQuery.GetContestantTotals();
            var secondContestantResult = secondResults.First();

            // Assert
            Assert.AreEqual(3, firstContestantResult.Score);
            Assert.AreEqual(4, secondContestantResult.Score);
        }
    }
}
