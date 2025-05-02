using Rubriki.Cqrs;
using Rubriki.Dto;

namespace Rubriki.SharedComponents;

public partial class ContestantPanel
{
    public class Model(SetupQuery setupQuery, ScoreQuery scoreQuery)
    {
        public Contestant? Contestant { get; private set; }
        public List<CriteriaScore> CriteriaScores { get; private set; } = [];

        public async Task Get(int contestantId)
        {
            Contestant = await setupQuery.GetContestant(contestantId);
            CriteriaScores =
            [
                .. (await scoreQuery.GetContestantCategoryScores(contestantId))
                    .OrderBy(x => x.Criteria.Category.Id)
                    .ThenBy(x => x.Criteria.Id)
            ];
        }
    }
}
