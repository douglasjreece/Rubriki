
namespace Rubriki.Interfaces;

public interface ISetupQuery
{
    Task<List<Category>> GetCategories();
    Task<Contestant> GetContestant(int contestantId);
    Task<List<Contestant>> GetContestants();
    Task<List<Criteria>> GetCriteria();
    Task<List<Criteria>> GetCriteria(int categoryId);
    Task<List<Judge>> GetJudges();
    Task<List<Level>> GetLevels();
}
