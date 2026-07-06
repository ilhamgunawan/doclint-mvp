using AutoMapper;
using DocLint.Application.DTOs;
using DocLint.Application.Interfaces;
using DocLint.Domain.Entities;
using DocLint.Domain.Enums;

namespace DocLint.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly ILintReportRepository _lintReportRepository;
    private readonly IMapper _mapper;
    private readonly IPdfDocumentExtractor _pdfExtractor;

    public DocumentService(
        IDocumentRepository documentRepository,
        ILintReportRepository lintReportRepository,
        IMapper mapper,
        IPdfDocumentExtractor pdfExtractor)
    {
        _documentRepository = documentRepository;
        _lintReportRepository = lintReportRepository;
        _mapper = mapper;
        _pdfExtractor = pdfExtractor;
    }

    public async Task<LintReportDto> LintDocumentAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var metadata = await _pdfExtractor.ExtractMetadataAsync(fileStream, cancellationToken);

        var document = new Document
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            FileSize = fileStream.Length,
            MimeType = "application/pdf",
            PageCount = metadata.PageCount,
            PageSize = metadata.PageSize,
            Orientation = metadata.Orientation
        };

        // await _documentRepository.AddAsync(document, cancellationToken);

        var report = new LintReport
        {
            Id = Guid.NewGuid(),
            DocumentId = document.Id,
            Status = LintStatus.Passed,
            RuleCount = 0,
            IssueCount = 0
        };

        // await _lintReportRepository.AddAsync(report, cancellationToken);

        var lintReport = new LintReportDto
        {
            Document = _mapper.Map<DocumentDto>(document),
            Summary = new SummaryDto
            {
                Status = report.Status.ToString(),
                RuleCount = report.RuleCount,
                IssueCount = report.IssueCount,
            },
        };

        return lintReport;
    }
}
