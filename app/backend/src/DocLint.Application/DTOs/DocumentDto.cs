namespace DocLint.Application.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string PageSize { get; set; } = string.Empty;
    public string Orientation { get; set; } = string.Empty;
}
