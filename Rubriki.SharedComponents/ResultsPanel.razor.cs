using Rubriki.Dto;
using Rubriki.Interfaces;

namespace Rubriki.SharedComponents;

public partial class ResultsPanel
{
    public class Model(IScoreQuery scoreQuery)
    {
        public List<ContestantTotalScore> ResultsList { get; private set; } = [];
        public async Task Get()
        {
            ResultsList = [.. (await scoreQuery.GetContestantTotals()).OrderByDescending(x => x.Score)];
        }
    }
}
