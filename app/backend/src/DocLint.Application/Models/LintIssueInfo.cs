using DocLint.Domain.Enums;

namespace DocLint.Application.Models;

public class LintIssueInfo
{
    public Severity Severity { get; set; }
    public int PageNumber { get; set; }
    public string Expected { get; set; } = string.Empty;
    public string Actual { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
