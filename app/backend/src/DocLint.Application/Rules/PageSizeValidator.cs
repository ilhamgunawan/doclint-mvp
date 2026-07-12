using DocLint.Application.Models;
using DocLint.Domain.Enums;

namespace DocLint.Application.Rules;

public class PageSizeValidator : IRuleValidator
{
    public string RuleType => "page_size";

    public IReadOnlyList<LintIssueInfo> Validate(RuleDefinition rule, DocumentModel document, IReadOnlyList<PageModel> targetPages)
    {
        var issues = new List<LintIssueInfo>();
        var expected = rule.Constraints.Size;
        var severity = ParseSeverity(rule.Severity);

        if (string.IsNullOrWhiteSpace(expected))
            return issues;

        foreach (var page in targetPages)
        {
            var actual = page.Size;

            if (string.IsNullOrWhiteSpace(actual) || !actual.Equals(expected, StringComparison.OrdinalIgnoreCase))
            {
                issues.Add(new LintIssueInfo
                {
                    Severity = severity,
                    PageNumber = page.PageNumber,
                    Expected = expected,
                    Actual = actual ?? "Unknown",
                    Message = $"Page {page.PageNumber} has size {actual ?? "Unknown"}, expected {expected}."
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
