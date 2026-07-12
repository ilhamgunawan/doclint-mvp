namespace DocLint.Application.Models;

public class TextBlockModel
{
    public string Text { get; set; } = string.Empty;
    public string FontFamily { get; set; } = string.Empty;
    public double FontSize { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}
