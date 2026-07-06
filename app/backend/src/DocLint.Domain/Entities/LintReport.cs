using DocLint.Domain.Enums;

namespace DocLint.Domain.Entities;

public class LintReport
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public LintStatus Status { get; set; }
    public int RuleCount { get; set; }
    public int IssueCount { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public Document Document { get; set; } = null!;
    public ICollection<LintIssue> Issues { get; set; } = new List<LintIssue>();
}
