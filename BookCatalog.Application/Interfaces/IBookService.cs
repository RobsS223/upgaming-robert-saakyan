using BookCatalog.Application.DTOs;
using BookCatalog.Application.Enums;

namespace BookCatalog.Application.Interfaces;

public interface IBookService
{
    Task<IReadOnlyList<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookDto>> GetAllBooksAsync(int? publicationYear, SortField? sortBy, CancellationToken cancellationToken = default);
    Task<BookDto> CreateBookAsync(BookDto bookDto, CancellationToken cancellationToken = default);
    Task<AuthorDto> GetAuthorDetailsAsync(int authorId, CancellationToken cancellationToken = default);
}