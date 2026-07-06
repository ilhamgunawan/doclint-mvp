namespace DocLint.Application.DTOs;

public class LintReportDto
{
    public DocumentDto Document { get; set; } = null!;
    public SummaryDto Summary { get; set; } = null!;
    public List<RuleResultDto> RuleResults { get; set; } = new();
    public List<LintIssueDto> Issues { get; set; } = new();
}
