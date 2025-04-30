using Rubriki.Dto;

namespace Rubriki.Website.Components.Pages
{
    public partial class ContestantPage
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
}
