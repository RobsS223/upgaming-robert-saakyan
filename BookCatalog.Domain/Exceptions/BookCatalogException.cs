namespace BookCatalog.Domain.Exceptions;

public abstract class BookCatalogException : Exception
{
    protected BookCatalogException(string message) : base(message) { }
}

public sealed class AuthorNotFoundException : BookCatalogException
{
    public AuthorNotFoundException(int authorId) 
        : base($"Author with ID {authorId} does not exist") { }
}

public sealed class InvalidBookDataException : BookCatalogException
{
    public InvalidBookDataException(string message) : base(message) { }
}