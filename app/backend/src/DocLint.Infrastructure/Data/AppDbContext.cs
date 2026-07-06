using DocLint.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocLint.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Document> Documents => Set<Document>();
    public DbSet<LintReport> LintReports => Set<LintReport>();
    public DbSet<LintIssue> LintIssues => Set<LintIssue>();
    public DbSet<RuleResult> RuleResults => Set<RuleResult>();
    public DbSet<LintRule> LintRules => Set<LintRule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.MimeType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PageSize).HasMaxLength(50);
            entity.Property(e => e.Orientation).HasMaxLength(50);
        });

        modelBuilder.Entity<LintReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Document)
                  .WithMany()
                  .HasForeignKey(e => e.DocumentId);
        });

        modelBuilder.Entity<LintIssue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RuleId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.RuleName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Severity).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.Expected).HasMaxLength(500);
            entity.Property(e => e.Actual).HasMaxLength(500);
            entity.Property(e => e.Message).HasMaxLength(1000).IsRequired();
            entity.HasOne(e => e.LintReport)
                  .WithMany(r => r.Issues)
                  .HasForeignKey(e => e.LintReportId);
        });

        modelBuilder.Entity<RuleResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RuleId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
            entity.HasOne(e => e.LintReport)
                  .WithMany()
                  .HasForeignKey(e => e.LintReportId);
        });

        modelBuilder.Entity<LintRule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
        });
    }
}
