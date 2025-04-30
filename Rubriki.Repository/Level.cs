using System.ComponentModel.DataAnnotations;

namespace Rubriki.Repository;

public class Level
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int Score { get; set; }
}
