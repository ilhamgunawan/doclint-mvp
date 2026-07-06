using DocLint.Application.Interfaces;
using DocLint.Application.Mapping;
using DocLint.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocLint.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IDocumentService, DocumentService>();

        return services;
    }
}
