using BookCatalog.Application.Interfaces;
using BookCatalog.Domain.Entities;
using BookCatalog.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Infrastructure.Repositories.Write;

public sealed class BookWriteRepository : IBookWriteRepository
{
    private readonly ApplicationDbContext _context;

    public BookWriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }
}
