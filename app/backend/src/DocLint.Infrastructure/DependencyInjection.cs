using DocLint.Application.Interfaces;
using DocLint.Infrastructure.Data;
using DocLint.Infrastructure.Repositories;
using DocLint.Infrastructure.Services.DocumentProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocLint.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ILintReportRepository, LintReportRepository>();
        services.AddScoped<IPdfDocumentExtractor, PdfPigDocumentExtractor>();

        return services;
    }
}
