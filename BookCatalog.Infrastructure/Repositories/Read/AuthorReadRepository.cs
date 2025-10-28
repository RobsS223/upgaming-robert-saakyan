using BookCatalog.Application.Interfaces;
using BookCatalog.Domain.Entities;
using BookCatalog.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Infrastructure.Repositories.Read;

public sealed class AuthorReadRepository : IAuthorReadRepository
{
    private readonly ApplicationDbContext _context;

    public AuthorReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
