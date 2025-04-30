namespace Rubriki.Dto;

public record ScoreEntry(Contestant Contestant, Judge Judge, Criteria Criteria, Level Level, string Comment);
