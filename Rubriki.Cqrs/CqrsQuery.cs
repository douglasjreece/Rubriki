using Rubriki.Dto;

namespace Rubriki.Cqrs;

public class CqrsQuery
{
    protected static Contestant Map(Repository.Contestant contestant)
    {
        return new Contestant(contestant.Id, contestant.Name);
    }

    protected static Judge Map(Repository.Judge judge)
    {
        return new Judge(judge.Id, judge.Name);
    }

    protected static Category Map(Repository.Category category)
    {
        return new Category(category.Id, category.Name);
    }

    protected static Criteria Map(Repository.Criteria criteria)
    {
        return new Criteria(new Category(criteria.Category.Id, criteria.Category.Name), criteria.Id, criteria.Name);
    }

    protected static Level Map(Repository.Level level)
    {
        return new Level(level.Id, level.Description, level.Score);
    }
}
