using BookCatalog.Domain.Entities;

namespace BookCatalog.Application.DTOs;

public sealed record AuthorDto(int Id, string Name, IReadOnlyList<Book> Books);
