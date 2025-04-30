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

            var command = new Cqrs.AdminCommand(context);
            var query = new Cqrs.ClientQuery(context);
            var seed = new Dto.SeedData
            {
                CategoryAndCriteria = 
                [
                    new("Category1", ["Criteria1", "Criteria2" ]),
                    new("Category2", [ "Criteria3" ])
                ],
                ContestantNames = ["Contestant1" ],
                JudgeNames = ["Judge1", "Judge2"]
            };

            // Act
            await command.SeedData(seed);
            var contestants = await query.GetContestantsAsync();
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
