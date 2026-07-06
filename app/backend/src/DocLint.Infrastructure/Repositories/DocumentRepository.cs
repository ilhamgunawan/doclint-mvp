using DocLint.Application.Interfaces;
using DocLint.Domain.Entities;
using DocLint.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocLint.Infrastructure.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly AppDbContext _context;

    public DocumentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Documents.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Document> AddAsync(Document document, CancellationToken cancellationToken = default)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync(cancellationToken);
        return document;
    }
}
