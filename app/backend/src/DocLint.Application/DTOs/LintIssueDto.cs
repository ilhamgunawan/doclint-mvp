namespace DocLint.Application.DTOs;

public class LintIssueDto
{
    public LintRuleDto Rule { get; set; } = null!;
    public string Severity { get; set; } = string.Empty;
    public int Page { get; set; }
    public string Expected { get; set; } = string.Empty;
    public string Actual { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
