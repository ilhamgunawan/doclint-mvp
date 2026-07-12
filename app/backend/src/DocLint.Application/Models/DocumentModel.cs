namespace DocLint.Application.Models;

public class DocumentModel
{
    public DocumentMetadata Metadata { get; set; } = null!;
    public IReadOnlyList<PageModel> Pages { get; set; } = Array.Empty<PageModel>();
}

public class DocumentMetadata
{
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string PageSize { get; set; } = string.Empty;
    public string Orientation { get; set; } = string.Empty;
}
