using UglyToad.PdfPig;

namespace DocLint.Infrastructure.Services.DocumentProcessing.PdfPig;

public class MetadataExtractor
{
    private const double Tolerance = 5.0;

    public DocumentMetadata Extract(PdfDocument document)
    {
        var pageCount = document.NumberOfPages;
        var pageSizes = new List<PageSizeInfo>(pageCount);

        for (var i = 1; i <= pageCount; i++)
        {
            var page = document.GetPage(i);
            pageSizes.Add(new PageSizeInfo { Width = (double)page.Width, Height = (double)page.Height });
        }

        return new DocumentMetadata
        {
            PageCount = pageCount,
            PageSize = GetMostCommonPageSize(document),
            Orientation = DetermineOrientation(pageSizes),
            PageSizes = pageSizes
        };
    }

    private static string GetMostCommonPageSize(PdfDocument document)
    {
        if (document.NumberOfPages == 0)
            return "Unknown";

        var sizes = new List<string>(document.NumberOfPages);

        for (var i = 1; i <= document.NumberOfPages; i++)
        {
            var page = document.GetPage(i);
            sizes.Add(page.Size.ToString());
        }

        return sizes
            .GroupBy(s => s)
            .OrderByDescending(g => g.Count())
            .First().Key;
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
