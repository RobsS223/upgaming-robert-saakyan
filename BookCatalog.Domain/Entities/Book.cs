namespace BookCatalog.Domain.Entities;

public sealed record Book(int Id, string Title, int AuthorId, int PublicationYear);