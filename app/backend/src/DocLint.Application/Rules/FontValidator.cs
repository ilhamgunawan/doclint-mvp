using DocLint.Application.Models;
using DocLint.Domain.Enums;

namespace DocLint.Application.Rules;

public class FontValidator : IRuleValidator
{
    public string RuleType => "font";

    public IReadOnlyList<LintIssueInfo> Validate(RuleDefinition rule, DocumentModel document, IReadOnlyList<PageModel> targetPages)
    {
        var issues = new List<LintIssueInfo>();
        var expectedFamily = rule.Constraints.FontFamily;
        var expectedSize = rule.Constraints.FontSize;
        var severity = ParseSeverity(rule.Severity);

        foreach (var page in targetPages)
        {
            if (page.TextBlocks.Count == 0)
                continue;

            if (!string.IsNullOrWhiteSpace(expectedFamily))
            {
                var allMatch = page.TextBlocks.All(b => FontMatches(b.FontFamily, expectedFamily));
                if (!allMatch)
                {
                    issues.Add(new LintIssueInfo
                    {
                        Severity = severity,
                        PageNumber = page.PageNumber,
                        Expected = expectedFamily,
                        Actual = "Mixed",
                        Message = $"Page {page.PageNumber} contains text using inconsistent font families."
                    });
                }
            }

            if (expectedSize != null)
            {
                var expectedPt = UnitConverter.ToPoints(expectedSize.Value, expectedSize.Unit);
                var allMatch = page.TextBlocks.All(b => Math.Abs(b.FontSize - expectedPt) <= 0.5);
                if (!allMatch)
                {
                    issues.Add(new LintIssueInfo
                    {
                        Severity = severity,
                        PageNumber = page.PageNumber,
                        Expected = $"{expectedPt:F0} pt",
                        Actual = "Mixed",
                        Message = $"Page {page.PageNumber} contains text using inconsistent font sizes."
                    });
                }
            }
        }

        return issues;
    }

    private static bool FontMatches(string actual, string expected)
    {
        if (string.IsNullOrWhiteSpace(actual))
            return false;

        var actualLower = actual.Replace("-", "").Replace("_", "").ToLowerInvariant();
        var expectedLower = expected.Replace("-", "").Replace("_", "").ToLowerInvariant();

        return actualLower.Contains(expectedLower);
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
