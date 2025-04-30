namespace Rubriki.Dto;

public record ScoreEntry(Contestant Contestant, Judge Judge, Criteria Criteria, int Score, string Comment);
