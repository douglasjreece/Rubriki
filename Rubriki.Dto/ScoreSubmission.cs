namespace Rubriki.Dto;

public record ScoreSubmission(int ContestantId, int JudgeId, int CriteriaId, int LevelId, string Comment);
