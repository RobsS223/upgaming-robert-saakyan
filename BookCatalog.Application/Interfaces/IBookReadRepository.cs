using BookCatalog.Domain.Entities;

namespace BookCatalog.Application.Interfaces;

public interface IBookReadRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken = default);
}
