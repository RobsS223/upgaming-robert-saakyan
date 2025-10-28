using BookCatalog.Domain.Entities;

namespace BookCatalog.Application.Interfaces;

public interface IBookWriteRepository
{
    Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default);
}
