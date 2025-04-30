using Rubriki.Dto;

namespace Rubriki.Website.Components.Pages
{
    public partial class ResultsPage
    {
        public class Model(Cqrs.AdminQuery query)
        {
            public List<ContestantTotalScore> ResultsList { get; private set; } = [];
            public async Task Get()
            {
                ResultsList = [..(await query.GetResults()).OrderByDescending(x => x.Score)];
            }
        }
    }
}
