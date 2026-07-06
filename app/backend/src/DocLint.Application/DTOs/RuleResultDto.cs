namespace DocLint.Application.DTOs;

public class RuleResultDto
{
    public string Rule { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int IssueCount { get; set; }
}
