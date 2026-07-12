namespace DocLint.Infrastructure.Services.DocumentProcessing.PdfPig;

public class PageContent
{
    public int PageNumber { get; set; }
    public string Text { get; set; } = string.Empty;
    public IReadOnlyList<TextBlock> TextBlocks { get; set; } = Array.Empty<TextBlock>();
}

public class TextBlock
{
    public string Text { get; set; } = string.Empty;
    public string FontName { get; set; } = string.Empty;
    public double FontSize { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}
