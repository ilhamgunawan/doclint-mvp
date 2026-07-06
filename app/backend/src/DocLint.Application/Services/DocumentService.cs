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

    public DocumentService(
        IDocumentRepository documentRepository,
        ILintReportRepository lintReportRepository,
        IMapper mapper)
    {
        _documentRepository = documentRepository;
        _lintReportRepository = lintReportRepository;
        _mapper = mapper;
    }

    public async Task<LintReportDto> LintDocumentAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var document = new Document
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            FileSize = fileStream.Length,
            MimeType = "application/pdf",
            PageCount = 0,
            PageSize = string.Empty,
            Orientation = string.Empty
        };

        await _documentRepository.AddAsync(document, cancellationToken);

        var report = new LintReport
        {
            Id = Guid.NewGuid(),
            DocumentId = document.Id,
            Status = LintStatus.Passed,
            RuleCount = 0,
            IssueCount = 0
        };

        await _lintReportRepository.AddAsync(report, cancellationToken);

        return _mapper.Map<LintReportDto>(report);
    }
}
