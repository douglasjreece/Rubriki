namespace Rubriki.Cqrs;

public class CqrsQuery
{
    protected static Dto.Contestant ToDto(Entities.Contestant contestant)
    {
        return new Dto.Contestant(contestant.Id, contestant.Name);
    }

    protected static Dto.Judge ToDto(Entities.Judge judge)
    {
        return new Dto.Judge(judge.Id, judge.Name);
    }

    protected static Dto.Category ToDto(Entities.Category category)
    {
        return new Dto.Category(category.Id, category.Name);
    }

    protected static Dto.Criteria ToDto(Entities.Criteria criteria)
    {
        return new Dto.Criteria(new Dto.Category(criteria.Category.Id, criteria.Category.Name), criteria.Id, criteria.Name);
    }

    protected static Dto.Level ToDto(Entities.Level level)
    {
        return new Dto.Level(level.Id, level.Description, level.Score);
    }
}
