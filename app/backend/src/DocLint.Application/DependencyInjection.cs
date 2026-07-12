using DocLint.Application.Interfaces;
using DocLint.Application.Mapping;
using DocLint.Application.Rules;
using DocLint.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocLint.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IDocumentService, DocumentLintService>();
        services.AddScoped<RuleEngine>();
        services.AddScoped<IRuleValidator, FontValidator>();
        services.AddScoped<IRuleValidator, PageMarginValidator>();
        services.AddScoped<IRuleValidator, PageSizeValidator>();
        services.AddScoped<IRuleValidator, PageOrientationValidator>();

        return services;
    }
}
