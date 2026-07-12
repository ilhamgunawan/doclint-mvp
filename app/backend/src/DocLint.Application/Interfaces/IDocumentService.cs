using DocLint.Application.DTOs;

namespace DocLint.Application.Interfaces;

public interface IDocumentService
{
    Task<LintReportDto> LintDocumentAsync(Stream fileStream, string fileName, string mimeType, long fileSize, CancellationToken cancellationToken = default);
}
