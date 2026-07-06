using DocLint.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocLint.WebAPI.Controllers;

[ApiController]
[Route("api/v1/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentsController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("lint")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<IActionResult> LintDocument(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = new { code = "INVALID_FILE", message = "No file provided." } });

        if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = new { code = "INVALID_FILE", message = "Only PDF files are supported." } });

        using var stream = file.OpenReadStream();
        var result = await _documentService.LintDocumentAsync(stream, file.FileName, cancellationToken);

        return Ok(result);
    }
}
