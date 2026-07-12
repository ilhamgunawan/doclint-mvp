using DocLint.Application.Models;

namespace DocLint.Infrastructure.Services.DocumentProcessing.PdfPig;

public class PdfPigMapper
{
    public DocumentModel Map(DocumentMetadata metadata, IReadOnlyList<PageContent> pageContents, long fileSize, string fileName, string mimeType)
    {
        var pages = pageContents.Select(pc => new PageModel
        {
            PageNumber = pc.PageNumber,
            TextBlocks = pc.TextBlocks.Select(tb => new TextBlockModel
            {
                Text = tb.Text,
                FontFamily = tb.FontName,
                FontSize = tb.FontSize,
                X = tb.X,
                Y = tb.Y,
                Width = tb.Width,
                Height = tb.Height
            }).ToList()
        }).ToList();

        return new DocumentModel
        {
            Metadata = new Application.Models.DocumentMetadata
            {
                FileName = fileName,
                FileSize = fileSize,
                MimeType = mimeType,
                PageCount = metadata.PageCount,
                PageSize = metadata.PageSize,
                Orientation = metadata.Orientation
            },
            Pages = pages
        };
    }
}
