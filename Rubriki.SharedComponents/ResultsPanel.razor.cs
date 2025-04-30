using Rubriki.Dto;

namespace Rubriki.SharedComponents;

public partial class ResultsPanel
{
    public class Model(Cqrs.ClientQuery query)
    {
        public List<ContestantTotalScore> ResultsList { get; private set; } = [];
        public async Task Get()
        {
            ResultsList = [.. (await query.GetResults()).OrderByDescending(x => x.Score)];
        }
    }
}
