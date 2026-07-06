using DocLint.Application.DTOs;
using DocLint.Application.Interfaces;
using UglyToad.PdfPig;

namespace DocLint.Infrastructure.Services.DocumentProcessing;

public class PdfPigDocumentExtractor : IPdfDocumentExtractor
{
    private static readonly (string Name, double Width, double Height)[] KnownSizes =
    {
        ("A0", 2383.94, 3370.39),
        ("A1", 1683.78, 2383.94),
        ("A2", 1190.55, 1683.78),
        ("A3", 841.89, 1191.10),
        ("A4", 595.28, 841.89),
        ("A5", 419.53, 595.28),
        ("A6", 297.64, 419.53),
        ("Letter", 612, 792),
        ("Legal", 612, 1008),
        ("Tabloid", 792, 1224),
        ("Executive", 522, 756),
    };

    private const double Tolerance = 5.0;

    public Task<PdfMetadata> ExtractMetadataAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var memoryStream = CopyToSeekableStream(stream);

        try
        {
            using var document = PdfDocument.Open(memoryStream);

            var pageCount = document.NumberOfPages;
            var pageSizes = new List<(double Width, double Height)>(pageCount);

            for (var i = 1; i <= pageCount; i++)
            {
                var page = document.GetPage(i);
                pageSizes.Add((page.Width, page.Height));
            }

            var mostCommonSize = GetMostCommonPageSize(pageSizes);
            var orientation = DetermineOrientation(pageSizes);

            return Task.FromResult(new PdfMetadata
            {
                PageCount = pageCount,
                PageSize = mostCommonSize,
                Orientation = orientation
            });
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

    public Task<IReadOnlyList<PdfPageContent>> ExtractPageContentsAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var memoryStream = CopyToSeekableStream(stream);

        try
        {
            using var document = PdfDocument.Open(memoryStream);

            var pageCount = document.NumberOfPages;
            var contents = new List<PdfPageContent>(pageCount);

            for (var i = 1; i <= pageCount; i++)
            {
                var page = document.GetPage(i);
                contents.Add(new PdfPageContent
                {
                    PageNumber = i,
                    Text = page.Text
                });
            }

            return Task.FromResult<IReadOnlyList<PdfPageContent>>(contents);
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

    private static string GetMostCommonPageSize(List<(double Width, double Height)> pageSizes)
    {
        if (pageSizes.Count == 0)
            return "Unknown";

        var normalized = pageSizes.Select(s => NormalizeSize(s.Width, s.Height)).ToList();
        var mostCommon = normalized
            .GroupBy(s => GetPaperSizeName(s.Width, s.Height))
            .OrderByDescending(g => g.Count())
            .First();

        return mostCommon.Key;
    }

    private static (double Width, double Height) NormalizeSize(double width, double height)
    {
        return width <= height ? (width, height) : (height, width);
    }

    private static string GetPaperSizeName(double width, double height)
    {
        foreach (var (name, knownWidth, knownHeight) in KnownSizes)
        {
            if (Math.Abs(width - knownWidth) <= Tolerance && Math.Abs(height - knownHeight) <= Tolerance)
                return name;
        }

        return $"Custom ({width:F0} x {height:F0})";
    }

    private static string DetermineOrientation(List<(double Width, double Height)> pageSizes)
    {
        if (pageSizes.Count == 0)
            return "Unknown";

        var orientations = pageSizes.Select(s =>
        {
            if (Math.Abs(s.Width - s.Height) < Tolerance)
                return "Square";
            return s.Width > s.Height ? "Landscape" : "Portrait";
        }).ToList();

        var mostCommon = orientations
            .GroupBy(o => o)
            .OrderByDescending(g => g.Count())
            .First().Key;

        return mostCommon;
    }
}
