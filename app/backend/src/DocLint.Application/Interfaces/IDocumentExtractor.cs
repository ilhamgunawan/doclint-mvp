using DocLint.Application.Models;

namespace DocLint.Application.Interfaces;

public interface IDocumentExtractor
{
    Task<DocumentModel> ExtractAsync(Stream stream, long fileSize, string fileName, string mimeType, CancellationToken cancellationToken = default);
}
