using DocLint.Domain.Enums;

namespace DocLint.Domain.Entities;

public class RuleResult
{
    public Guid Id { get; set; }
    public Guid LintReportId { get; set; }
    public string RuleId { get; set; } = string.Empty;
    public LintStatus Status { get; set; }
    public int IssueCount { get; set; }

    public LintReport LintReport { get; set; } = null!;
}
