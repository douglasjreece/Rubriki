using Rubriki.Dto;

namespace Rubriki.SharedComponents;

public partial class ContestantPanel
{
    public class Model(Cqrs.ClientQuery query)
    {
        public Contestant? Contestant { get; private set; }
        public List<CriteriaScore> CriteriaScores { get; private set; } = [];

        public async Task Get(int contestantId)
        {
            Contestant = await query.GetContestant(contestantId);
            CriteriaScores =
            [
                .. (await query.GetContestantCategoryScores(contestantId))
                    .OrderBy(x => x.Criteria.Category.Id)
                    .ThenBy(x => x.Criteria.Id)
            ];
        }
    }
}
