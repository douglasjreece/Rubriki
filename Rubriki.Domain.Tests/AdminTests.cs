using Microsoft.EntityFrameworkCore;

namespace Rubriki.Domain.Tests
{
    [TestClass]
    public sealed class AdminTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Repository.ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var context = new Repository.ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var command = new Cqrs.SetupCommand(context);
            var query = new Cqrs.ClientQuery(context);
            var seed = new Dto.SeedData
            {
                CategoryAndCriteria =
                {
                    { "Category1",["Criteria1", "Criteria2"] },
                    { "Category2",["Criteria3"] }
                },
                ContestantNames = ["Contestant1" ],
                JudgeNames = ["Judge1", "Judge2"]
            };

            // Act
            await command.Seed(seed);
            var contestants = await query.GetContestants();
            var contestant = contestants.FirstOrDefault();
            var contestantId = contestant?.Id;
            var contestantName = contestant?.Name;
            var contestantCount = contestants.Count;

            // Assert
            Assert.IsNotNull(contestant);
            Assert.AreEqual("Contestant1", contestantName);
            Assert.AreEqual(1, contestantCount);
            Assert.IsTrue(contestantId > 0);
        }
    }
}
