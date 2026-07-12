using DocLint.Application.Interfaces;
using DocLint.Application.Models;
using UglyToad.PdfPig;

namespace DocLint.Infrastructure.Services.DocumentProcessing;

public class PdfPigDocumentExtractor : IDocumentExtractor
{
    private readonly PdfPig.MetadataExtractor _metadataExtractor = new();
    private readonly PdfPig.ContentExtractor _contentExtractor = new();
    private readonly PdfPig.PdfPigMapper _mapper = new();

    public Task<DocumentModel> ExtractAsync(Stream stream, long fileSize, string fileName, string mimeType, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var memoryStream = CopyToSeekableStream(stream);

        try
        {
            using var document = PdfDocument.Open(memoryStream);

            var metadata = _metadataExtractor.Extract(document);
            var pageContents = _contentExtractor.Extract(document);
            var result = _mapper.Map(metadata, pageContents, fileSize, fileName, mimeType);

            return Task.FromResult(result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("encrypted", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Password-protected PDFs are not supported.", ex);
        }
        catch (Exception ex) when (ex is not UnauthorizedAccessException)
        {
            throw new InvalidDataException("The PDF file is invalid or corrupted.", ex);
        }
    }

    private static MemoryStream CopyToSeekableStream(Stream stream)
    {
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
}
