using DocLint.Application.DTOs;
using DocLint.Application.Interfaces;
using DocLint.Application.Rules;
using DocLint.Domain.Enums;

namespace DocLint.Application.Services;

public class DocumentLintService : IDocumentService
{
    private readonly IDocumentExtractor _documentExtractor;
    private readonly RuleEngine _ruleEngine;

    public DocumentLintService(IDocumentExtractor documentExtractor, RuleEngine ruleEngine)
    {
        _documentExtractor = documentExtractor;
        _ruleEngine = ruleEngine;
    }

    public async Task<LintReportDto> LintDocumentAsync(Stream fileStream, string fileName, string mimeType, long fileSize, CancellationToken cancellationToken = default)
    {
        var document = await _documentExtractor.ExtractAsync(fileStream, fileSize, fileName, mimeType, cancellationToken);

        var ruleResults = await _ruleEngine.EvaluateAsync(document, cancellationToken);

        var issuesWithRule = ruleResults
            .SelectMany(r => r.Issues.Select(i => (RuleId: r.RuleId, RuleName: r.RuleName, Issue: i)))
            .ToList();

        var allFailed = issuesWithRule.Count > 0;

        return new LintReportDto
        {
            Document = new DocumentDto
            {
                Id = Guid.NewGuid(),
                FileName = document.Metadata.FileName,
                FileSize = document.Metadata.FileSize,
                MimeType = document.Metadata.MimeType,
                PageCount = document.Metadata.PageCount,
                PageSize = document.Metadata.PageSize,
                Orientation = document.Metadata.Orientation
            },
            Summary = new SummaryDto
            {
                Status = allFailed ? LintStatus.Failed.ToString() : LintStatus.Passed.ToString(),
                RuleCount = ruleResults.Count,
                IssueCount = issuesWithRule.Count,
            },
            RuleResults = ruleResults.Select(r => new RuleResultDto
            {
                Rule = r.RuleName,
                Status = r.Status.ToString(),
                IssueCount = r.Issues.Count,
            }).ToList(),
            Issues = issuesWithRule.Select(x => new LintIssueDto
            {
                Rule = new LintRuleDto { Id = x.RuleId, Name = x.RuleName },
                Severity = x.Issue.Severity.ToString(),
                Page = x.Issue.PageNumber,
                Expected = x.Issue.Expected,
                Actual = x.Issue.Actual,
                Message = x.Issue.Message,
            }).ToList(),
        };
    }
}
