using DocLint.Domain.Enums;

namespace DocLint.Application.Models;

public class RuleEvaluationResult
{
    public string RuleId { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public LintStatus Status { get; set; }
    public IReadOnlyList<LintIssueInfo> Issues { get; set; } = Array.Empty<LintIssueInfo>();
}
