using DocLint.Application.Models;
using DocLint.Domain.Enums;

namespace DocLint.Application.Rules;

public class PageSizeValidator : IRuleValidator
{
    public string RuleType => "page_size";

    public IReadOnlyList<LintIssueInfo> Validate(RuleDefinition rule, DocumentModel document, IReadOnlyList<PageModel> targetPages)
    {
        var issues = new List<LintIssueInfo>();
        var c = rule.Constraints;
        var severity = ParseSeverity(rule.Severity);

        var expectedWidth = c.Width != null ? UnitConverter.ToPoints(c.Width.Value, c.Width.Unit) : 0;
        var expectedHeight = c.Height != null ? UnitConverter.ToPoints(c.Height.Value, c.Height.Unit) : 0;

        foreach (var page in targetPages)
        {
            if (Math.Abs(page.Width - expectedWidth) > 0.5 || Math.Abs(page.Height - expectedHeight) > 0.5)
            {
                issues.Add(new LintIssueInfo
                {
                    Severity = severity,
                    PageNumber = page.PageNumber,
                    Expected = $"Page size: {expectedWidth:F0} x {expectedHeight:F0} pt",
                    Actual = $"{page.Width:F0} x {page.Height:F0} pt",
                    Message = $"Page {page.PageNumber} has size {page.Width:F0} x {page.Height:F0} pt, expected {expectedWidth:F0} x {expectedHeight:F0} pt."
                });
            }
        }

        return issues;
    }

    private static Severity ParseSeverity(string severity)
    {
        return severity.ToLowerInvariant() switch
        {
            "error" => Severity.Error,
            "warning" => Severity.Warning,
            _ => Severity.Warning
        };
    }
}
