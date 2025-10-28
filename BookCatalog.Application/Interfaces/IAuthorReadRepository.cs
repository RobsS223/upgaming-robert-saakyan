using BookCatalog.Domain.Entities;

namespace BookCatalog.Application.Interfaces;

public interface IAuthorReadRepository
{
    Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default);
}
