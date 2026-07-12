using DocLint.Application.Models;
using DocLint.Domain.Enums;

namespace DocLint.Application.Rules;

public class PageMarginValidator : IRuleValidator
{
    public string RuleType => "page_margin";

    public IReadOnlyList<LintIssueInfo> Validate(RuleDefinition rule, DocumentModel document, IReadOnlyList<PageModel> targetPages)
    {
        var issues = new List<LintIssueInfo>();
        var c = rule.Constraints;
        var severity = ParseSeverity(rule.Severity);

        var topMargin = c.Top != null ? UnitConverter.ToPoints(c.Top.Value, c.Top.Unit) : 0;
        var bottomMargin = c.Bottom != null ? UnitConverter.ToPoints(c.Bottom.Value, c.Bottom.Unit) : 0;
        var leftMargin = c.Left != null ? UnitConverter.ToPoints(c.Left.Value, c.Left.Unit) : 0;
        var rightMargin = c.Right != null ? UnitConverter.ToPoints(c.Right.Value, c.Right.Unit) : 0;

        foreach (var page in targetPages)
        {
            if (page.TextBlocks.Count == 0)
                continue;

            var violatedSides = new List<string>();

            foreach (var block in page.TextBlocks)
            {
                var actualLeft = block.X;
                var actualBottom = block.Y;
                var actualTop = page.Height - (block.Y + block.Height);
                var actualRight = page.Width - (block.X + block.Width);

                if (actualLeft < leftMargin - 0.5)
                    violatedSides.Add("left");
                if (actualRight < rightMargin - 0.5)
                    violatedSides.Add("right");
                if (actualTop < topMargin - 0.5)
                    violatedSides.Add("top");
                if (actualBottom < bottomMargin - 0.5)
                    violatedSides.Add("bottom");
            }

            if (violatedSides.Count > 0)
            {
                var unique = violatedSides.Distinct().ToList();
                issues.Add(new LintIssueInfo
                {
                    Severity = severity,
                    PageNumber = page.PageNumber,
                    Expected = $"{FormatMargin(leftMargin, "left")}, {FormatMargin(rightMargin, "right")}, {FormatMargin(topMargin, "top")}, {FormatMargin(bottomMargin, "bottom")}",
                    Actual = $"Less than required ({string.Join(", ", unique)})",
                    Message = $"Page {page.PageNumber} has margins smaller than required on: {string.Join(", ", unique)}."
                });
            }
        }

        return issues;
    }

    private static string FormatMargin(double points, string side)
    {
        return $"{side}={points:F0} pt";
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
