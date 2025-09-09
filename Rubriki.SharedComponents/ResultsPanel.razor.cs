using Rubriki.Dto;
using Rubriki.Interfaces;

namespace Rubriki.SharedComponents;

public partial class ResultsPanel
{
    public class Model(IScoreQuery scoreQuery, ISetupQuery setupQuery)
    {
        public List<ContestantTotalScore> TotalsList { get; private set; } = [];
        public List<CategoryScores> ByCategory { get; private set; } = [];
        public async Task Get()
        {
            TotalsList = [.. (await scoreQuery.GetContestantTotals()).OrderByDescending(x => x.Score)];

            var categories = await setupQuery.GetCategories();
            ByCategory = categories.Select(category => new CategoryScores(category, scoreQuery.GetContestantTotalsForCategory(category.Id).Result)).ToList();
        }

        public record CategoryScores(Category Category, List<ContestantTotalScore> Scores);
    }
}
