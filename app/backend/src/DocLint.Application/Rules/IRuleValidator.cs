using DocLint.Application.Models;

namespace DocLint.Application.Rules;

public interface IRuleValidator
{
    string RuleType { get; }
    IReadOnlyList<LintIssueInfo> Validate(RuleDefinition rule, DocumentModel document, IReadOnlyList<PageModel> targetPages);
}
