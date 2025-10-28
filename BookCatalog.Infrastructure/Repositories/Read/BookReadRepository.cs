using BookCatalog.Application.Interfaces;
using BookCatalog.Domain.Entities;
using BookCatalog.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Infrastructure.Repositories.Read;

public sealed class BookReadRepository : IBookReadRepository
{
    private readonly ApplicationDbContext _context;

    public BookReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(b => b.AuthorId == authorId)
            .ToListAsync(cancellationToken);
    }
}
