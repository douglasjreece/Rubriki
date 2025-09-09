using System.ComponentModel.DataAnnotations;

namespace Rubriki.Entities;

public class Level
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int Score { get; set; }

    // Navigation property
    public ICollection<Score>? Results { get; set; }
}
