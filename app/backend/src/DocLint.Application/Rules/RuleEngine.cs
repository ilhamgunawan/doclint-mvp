using DocLint.Application.Models;
using DocLint.Domain.Enums;

namespace DocLint.Application.Rules;

public class RuleEngine
{
    private readonly IReadOnlyList<RuleDefinition> _rules;
    private readonly Dictionary<string, IRuleValidator> _validators;

    public RuleEngine(IEnumerable<RuleDefinition> rules, IEnumerable<IRuleValidator> validators)
    {
        _rules = rules.Where(r => r.IsActive).ToList();
        _validators = validators.ToDictionary(v => v.RuleType, StringComparer.OrdinalIgnoreCase);
    }

    public Task<IReadOnlyList<RuleEvaluationResult>> EvaluateAsync(DocumentModel document, CancellationToken cancellationToken = default)
    {
        var results = new List<RuleEvaluationResult>();

        foreach (var rule in _rules)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!_validators.TryGetValue(rule.Type, out var validator))
                continue;

            var targetPages = PageSelectorResolver.Resolve(rule.PageSelector, document);
            var issues = validator.Validate(rule, document, targetPages);

            var status = issues.Count == 0 ? LintStatus.Passed : LintStatus.Failed;

            results.Add(new RuleEvaluationResult
            {
                RuleId = rule.Id.ToString(),
                RuleName = rule.Name,
                Status = status,
                Issues = issues
            });
        }

        return Task.FromResult<IReadOnlyList<RuleEvaluationResult>>(results);
    }
}
