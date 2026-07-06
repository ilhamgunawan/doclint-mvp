using System.Net;
using System.Text.Json;

namespace DocLint.WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, code, message) = exception switch
        {
            FileNotFoundException => (HttpStatusCode.BadRequest, "INVALID_FILE", exception.Message),
            InvalidDataException => (HttpStatusCode.UnprocessableEntity, "INVALID_PDF", exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.UnprocessableEntity, "PDF_ENCRYPTED", "Password-protected PDFs are not supported."),
            _ => (HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            error = new
            {
                code,
                message
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
