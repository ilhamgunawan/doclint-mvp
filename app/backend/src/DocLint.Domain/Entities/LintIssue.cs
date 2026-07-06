using DocLint.Domain.Enums;

namespace DocLint.Domain.Entities;

public class LintIssue
{
    public Guid Id { get; set; }
    public Guid LintReportId { get; set; }
    public string RuleId { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public Severity Severity { get; set; }
    public int PageNumber { get; set; }
    public string Expected { get; set; } = string.Empty;
    public string Actual { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public LintReport LintReport { get; set; } = null!;
}
