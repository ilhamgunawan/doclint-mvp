namespace DocLint.Application.DTOs;

public class PdfMetadata
{
    public int PageCount { get; set; }
    public string PageSize { get; set; } = string.Empty;
    public string Orientation { get; set; } = string.Empty;
}
