namespace Rubriki.Dto;

public class SeedData
{
    public List<string> ContestantNames { get; set; } = [];
    public List<string> JudgeNames { get; set; } = [];
    public Dictionary<string, List<string>> CategoryAndCriteria { get; set; } = [];
    public List<string> Levels { get; set; } = [];
}
