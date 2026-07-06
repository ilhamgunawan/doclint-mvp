using DocLint.Application.DTOs;

namespace DocLint.Application.Interfaces;

public interface IPdfDocumentExtractor
{
    Task<PdfMetadata> ExtractMetadataAsync(Stream stream, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PdfPageContent>> ExtractPageContentsAsync(Stream stream, CancellationToken cancellationToken = default);
}
