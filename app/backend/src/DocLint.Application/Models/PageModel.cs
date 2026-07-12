namespace DocLint.Application.Models;

public class PageModel
{
    public int PageNumber { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public string Size { get; set; }
    public IReadOnlyList<TextBlockModel> TextBlocks { get; set; } = Array.Empty<TextBlockModel>();
}
