using Rubriki.Cqrs;
using Rubriki.Dto;

namespace Rubriki.SharedComponents;

public partial class ResultsPanel
{
    public class Model(ScoreQuery scoreQuery)
    {
        public List<ContestantTotalScore> ResultsList { get; private set; } = [];
        public async Task Get()
        {
            ResultsList = [.. (await scoreQuery.GetContestantTotals()).OrderByDescending(x => x.Score)];
        }
    }
}
