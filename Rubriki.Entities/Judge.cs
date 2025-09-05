using System.ComponentModel.DataAnnotations;

namespace Rubriki.Entities;

public class Judge
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Score>? Results { get; set; }
}