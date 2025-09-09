using Rubriki.Dto;
using Rubriki.Interfaces;

namespace Rubriki.SharedComponents;

public partial class ContestantPanel
{
    public class Model(ISetupQuery setupQuery, IScoreQuery scoreQuery)
    {
        public Contestant? Contestant { get; private set; }
        public List<CriteriaScore> CriteriaScores { get; private set; } = [];

        public async Task Get(int contestantId)
        {
            Contestant = await setupQuery.GetContestant(contestantId);
            CriteriaScores =
            [
                .. (await scoreQuery.GetContestantCriteriaScores(contestantId))
                    .OrderBy(x => x.Criteria.Category.Id)
                    .ThenBy(x => x.Criteria.Id)
            ];
        }
    }
}
