using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rubriki.Entities;

public class Score
{
    [Key]
    [Column(Order = 1)]
    public int ContestantId { get; set; }

    [Key]
    [Column(Order = 2)]
    public int CriteriaId { get; set; }

    [Key]
    [Column(Order = 3)]
    public int JudgeId { get; set; }

    [Required]
    public int LevelId { get; set; }

    [Required]
    public string Comment { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey("ContestantId")]
    public Contestant Contestant { get; set; } = default!;

    [ForeignKey("CriteriaId")]
    public Criteria Criteria { get; set; } = default!;

    [ForeignKey("JudgeId")]
    public Judge Judge { get; set; } = default!;

    [ForeignKey("LevelId")]
    public Level Level { get; set; } = default!;
}