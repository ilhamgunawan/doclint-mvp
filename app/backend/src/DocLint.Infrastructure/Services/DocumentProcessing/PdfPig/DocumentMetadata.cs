namespace DocLint.Infrastructure.Services.DocumentProcessing.PdfPig;

public class DocumentMetadata
{
    public int PageCount { get; set; }
    public string PageSize { get; set; } = string.Empty;
    public string Orientation { get; set; } = string.Empty;
    public IReadOnlyList<PageSizeInfo> PageSizes { get; set; } = Array.Empty<PageSizeInfo>();
}

public class PageSizeInfo
{
    public double Width { get; set; }
    public double Height { get; set; }
}
