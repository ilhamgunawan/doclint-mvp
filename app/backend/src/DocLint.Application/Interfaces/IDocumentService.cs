using DocLint.Application.DTOs;

namespace DocLint.Application.Interfaces;

public interface IDocumentService
{
    Task<LintReportDto> LintDocumentAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}
