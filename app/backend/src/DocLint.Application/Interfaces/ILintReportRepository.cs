using DocLint.Domain.Entities;

namespace DocLint.Application.Interfaces;

public interface ILintReportRepository
{
    Task<LintReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LintReport> AddAsync(LintReport report, CancellationToken cancellationToken = default);
}
