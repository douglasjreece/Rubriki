using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubriki.Domain.Tests
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
            var adminQuery = new Cqrs.AdminQuery(context);
            var clientCommand = new Cqrs.ClientCommand(context);
            var clientQuery = new Cqrs.ClientQuery(context);
            var seed = new Dto.SeedData
            {
                CategoryAndCriteria =
                {
                    { "Category1", ["Criteria1", "Criteria2" ] },
                    { "Category2", [ "Criteria3" ] }
                },
                ContestantNames = ["Contestant1"],
                JudgeNames = ["Judge1", "Judge2"]
            };

            // Act
            await adminCommand.Seed(seed);
            var contestants = await clientQuery.GetContestants();
            var contestant = contestants.First();
            var judges = await clientQuery.GetJudges();
            var judge = judges.First();
            var categories = await clientQuery.GetCategories();
            var category = categories.First();
            var criterias = await clientQuery.GetCriteria(category.Id);
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[0].Id, 1, "");
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[1].Id, 2, "");
            var results = await adminQuery.GetResults();
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
            var adminQuery = new Cqrs.AdminQuery(context);
            var clientCommand = new Cqrs.ClientCommand(context);
            var clientQuery = new Cqrs.ClientQuery(context);
            var seed = new Dto.SeedData
            {
                CategoryAndCriteria =
                {
                    { "Category1", ["Criteria1", "Criteria2" ]},
                    { "Category2", [ "Criteria3" ] }
                },
                ContestantNames = ["Contestant1"],
                JudgeNames = ["Judge1", "Judge2"]
            };

            // Act
            await adminCommand.Seed(seed);
            var contestants = await clientQuery.GetContestants();
            var contestant = contestants.First();
            var judges = await clientQuery.GetJudges();
            var judge = judges.First();
            var categories = await clientQuery.GetCategories();
            var category = categories.First();
            var criterias = await clientQuery.GetCriteria(category.Id);
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[0].Id, 1, "");
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[1].Id, 2, "");
            var firstResults = await adminQuery.GetResults();
            var firstContestantResult = firstResults.First();
            await clientCommand.SetScore(contestant.Id, judge.Id, criterias[1].Id, 3, "");
            var secondResults = await adminQuery.GetResults();
            var secondContestantResult = secondResults.First();

            // Assert
            Assert.AreEqual(3, firstContestantResult.Score);
            Assert.AreEqual(4, secondContestantResult.Score);
        }
    }
}
