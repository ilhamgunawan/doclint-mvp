namespace DocLint.Application.DTOs;

public class SummaryDto
{
    public string Status { get; set; } = string.Empty;
    public int RuleCount { get; set; }
    public int IssueCount { get; set; }
}
