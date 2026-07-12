using DocLint.Application.Models;

namespace DocLint.Application.Rules;

public static class PageSelectorResolver
{
    public static IReadOnlyList<PageModel> Resolve(PageSelectorModel selector, DocumentModel document)
    {
        if (string.IsNullOrWhiteSpace(selector.Type))
            return Array.Empty<PageModel>();

        if (selector.Type == "all")
            return document.Pages;

        if (selector.Type == "range")
        {
            var start = selector.Start ?? 1;
            var end = selector.End ?? document.Pages.Count;
            return document.Pages
                .Where(p => p.PageNumber >= start && p.PageNumber <= end)
                .ToList();
        }

        return Array.Empty<PageModel>();
    }
}
