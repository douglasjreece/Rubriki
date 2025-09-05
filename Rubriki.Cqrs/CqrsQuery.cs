namespace Rubriki.Cqrs;

public class CqrsQuery
{
    protected static Contestant ToDto(Repository.Contestant contestant)
    {
        return new Contestant(contestant.Id, contestant.Name);
    }

    protected static Judge ToDto(Repository.Judge judge)
    {
        return new Judge(judge.Id, judge.Name);
    }

    protected static Category ToDto(Repository.Category category)
    {
        return new Category(category.Id, category.Name);
    }

    protected static Criteria ToDto(Repository.Criteria criteria)
    {
        return new Criteria(new Category(criteria.Category.Id, criteria.Category.Name), criteria.Id, criteria.Name);
    }

    protected static Level ToDto(Repository.Level level)
    {
        return new Level(level.Id, level.Description, level.Score);
    }
}
