namespace Rubriki.Cqrs;

public static class Map
{
    public static Dto.Contestant ToDto(Entities.Contestant contestant) => new(contestant.Id, contestant.Name);

    public static Dto.Judge ToDto(Entities.Judge judge) => new(judge.Id, judge.Name);

    public static Dto.Category ToDto(Entities.Category category) => new(category.Id, category.Name);

    public static Dto.Criteria ToDto(Entities.Criteria criteria) => new(new Dto.Category(criteria.Category.Id, criteria.Category.Name), criteria.Id, criteria.Name);

    public static Dto.Level ToDto(Entities.Level level) => new(level.Id, level.Description, level.Score);
}
