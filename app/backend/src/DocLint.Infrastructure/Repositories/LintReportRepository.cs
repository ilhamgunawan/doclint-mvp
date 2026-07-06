using DocLint.Application.Interfaces;
using DocLint.Domain.Entities;
using DocLint.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocLint.Infrastructure.Repositories;

public class LintReportRepository : ILintReportRepository
{
    private readonly AppDbContext _context;

    public LintReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LintReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.LintReports
            .Include(r => r.Document)
            .Include(r => r.Issues)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<LintReport> AddAsync(LintReport report, CancellationToken cancellationToken = default)
    {
        _context.LintReports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);
        return report;
    }
}
