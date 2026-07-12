using DocLint.Application.Models;
using DocLint.Domain.Enums;

namespace DocLint.Application.Rules;

public class PageOrientationValidator : IRuleValidator
{
    private const double Tolerance = 5.0;

    public string RuleType => "page_orientation";

    public IReadOnlyList<LintIssueInfo> Validate(RuleDefinition rule, DocumentModel document, IReadOnlyList<PageModel> targetPages)
    {
        var issues = new List<LintIssueInfo>();
        var expectedOrientation = rule.Constraints.Orientation;
        var severity = ParseSeverity(rule.Severity);

        if (string.IsNullOrWhiteSpace(expectedOrientation))
            return issues;

        expectedOrientation = expectedOrientation.ToLowerInvariant();

        foreach (var page in targetPages)
        {
            var actual = GetOrientation(page);

            if (!actual.Equals(expectedOrientation, StringComparison.OrdinalIgnoreCase))
            {
                issues.Add(new LintIssueInfo
                {
                    Severity = severity,
                    PageNumber = page.PageNumber,
                    Expected = expectedOrientation,
                    Actual = actual,
                    Message = $"Page {page.PageNumber} has {actual} orientation, expected {expectedOrientation}."
                });
            }
        }

        return issues;
    }

    private static string GetOrientation(PageModel page)
    {
        if (Math.Abs(page.Width - page.Height) < Tolerance)
            return "square";

        return page.Width > page.Height ? "landscape" : "portrait";
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
