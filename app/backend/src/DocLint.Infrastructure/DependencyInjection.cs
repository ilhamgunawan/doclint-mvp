using System.Text.Json;
using DocLint.Application.Interfaces;
using DocLint.Application.Models;
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
        services.AddScoped<IDocumentExtractor, PdfPigDocumentExtractor>();

        var rulesPath = Path.Combine(AppContext.BaseDirectory, "Configuration", "default_rules.json");
        if (File.Exists(rulesPath))
        {
            var json = File.ReadAllText(rulesPath);
            var collection = JsonSerializer.Deserialize<RuleCollection>(json);
            if (collection?.Rules != null)
            {
                foreach (var rule in collection.Rules)
                {
                    services.AddSingleton(rule);
                }
            }
        }

        return services;
    }
}
