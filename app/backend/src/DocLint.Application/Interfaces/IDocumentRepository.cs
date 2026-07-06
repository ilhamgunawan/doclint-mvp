using DocLint.Domain.Entities;

namespace DocLint.Application.Interfaces;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Document> AddAsync(Document document, CancellationToken cancellationToken = default);
}
