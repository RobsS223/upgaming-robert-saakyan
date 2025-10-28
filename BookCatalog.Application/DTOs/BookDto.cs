namespace BookCatalog.Application.DTOs;

public sealed record BookDto(int? Id, string Title, int AuthorId, string? AuthorName, int PublicationYear);