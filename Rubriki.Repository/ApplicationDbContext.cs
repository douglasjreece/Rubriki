using Microsoft.EntityFrameworkCore;
using Rubriki.Entities;

namespace Rubriki.Repository;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Contestant> Contestants { get; set; }
    public DbSet<Criteria> Criteria { get; set; }
    public DbSet<Judge> Judges { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Score> Scores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure composite key for Result
        modelBuilder.Entity<Score>()
            .HasKey(r => new { r.ContestantId, r.CriteriaId, r.JudgeId });

        // Configure relationships
        modelBuilder.Entity<Score>()
            .HasOne(r => r.Contestant)
            .WithMany(c => c.Results)
            .HasForeignKey(r => r.ContestantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Score>()
            .HasOne(r => r.Criteria)
            .WithMany(c => c.Results)
            .HasForeignKey(r => r.CriteriaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Score>()
            .HasOne(r => r.Judge)
            .WithMany(j => j.Results)
            .HasForeignKey(r => r.JudgeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Score>()
            .HasOne(r => r.Level)
            .WithMany(j => j.Results)
            .HasForeignKey(r => r.LevelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Criteria>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Criteria)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
