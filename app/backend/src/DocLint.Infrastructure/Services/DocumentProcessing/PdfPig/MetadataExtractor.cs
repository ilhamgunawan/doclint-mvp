using UglyToad.PdfPig;

namespace DocLint.Infrastructure.Services.DocumentProcessing.PdfPig;

public class MetadataExtractor
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

    public DocumentMetadata Extract(PdfDocument document)
    {
        var pageCount = document.NumberOfPages;
        var pageSizes = new List<PageSizeInfo>(pageCount);

        for (var i = 1; i <= pageCount; i++)
        {
            var page = document.GetPage(i);
            pageSizes.Add(new PageSizeInfo { Width = page.Width, Height = page.Height });
        }

        return new DocumentMetadata
        {
            PageCount = pageCount,
            PageSize = GetMostCommonPageSize(pageSizes),
            Orientation = DetermineOrientation(pageSizes),
            PageSizes = pageSizes
        };
    }

    private static string GetMostCommonPageSize(List<PageSizeInfo> pageSizes)
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

    private static string DetermineOrientation(List<PageSizeInfo> pageSizes)
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
