using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rubriki.Repository;

public class Criteria
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    // Navigation properties
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public ICollection<Score>? Results { get; set; }
}
