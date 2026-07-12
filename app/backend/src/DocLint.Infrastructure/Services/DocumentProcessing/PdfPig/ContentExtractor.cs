using UglyToad.PdfPig;

namespace DocLint.Infrastructure.Services.DocumentProcessing.PdfPig;

public class ContentExtractor
{
    public IReadOnlyList<PageContent> Extract(PdfDocument document)
    {
        var pages = new List<PageContent>(document.NumberOfPages);

        for (var i = 1; i <= document.NumberOfPages; i++)
        {
            var page = document.GetPage(i);
            var textBlocks = new List<TextBlock>();

            foreach (var word in page.GetWords())
            {
                if (word.Letters.Count == 0)
                    continue;

                var letter = word.Letters[0];
                var bbox = word.BoundingBox;

                textBlocks.Add(new TextBlock
                {
                    Text = word.Text,
                    FontName = letter.FontName,
                    FontSize = letter.PointSize,
                    X = bbox.Left,
                    Y = bbox.Bottom,
                    Width = bbox.Width,
                    Height = bbox.Height
                });
            }

            pages.Add(new PageContent
            {
                PageNumber = i,
                Text = page.Text,
                TextBlocks = textBlocks
            });
        }

        return pages;
    }
}
