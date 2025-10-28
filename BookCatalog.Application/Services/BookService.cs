using BookCatalog.Application.DTOs;
using BookCatalog.Application.Enums;
using BookCatalog.Application.Interfaces;
using BookCatalog.Domain.Entities;
using BookCatalog.Domain.Exceptions;
using FluentValidation;

namespace BookCatalog.Application.Services;

public sealed class BookService : IBookService
{
    private readonly IBookReadRepository _bookReadRepository;
    private readonly IBookWriteRepository _bookWriteRepository;
    private readonly IAuthorReadRepository _authorReadRepository;
    private readonly IValidator<BookDto> _validator;

    public BookService(
        IBookReadRepository bookReadRepository,
        IBookWriteRepository bookWriteRepository,
        IAuthorReadRepository authorReadRepository,
        IValidator<BookDto> validator)
    {
        _bookReadRepository = bookReadRepository;
        _bookWriteRepository = bookWriteRepository;
        _authorReadRepository = authorReadRepository;
        _validator = validator;
    }

    public async Task<IReadOnlyList<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        var books = await _bookReadRepository.GetAllAsync(cancellationToken);
        var authors = await _authorReadRepository.GetAllAsync(cancellationToken);
        return MapBooksToDtos(books, authors);
    }

    public async Task<IReadOnlyList<BookDto>> GetAllBooksAsync(int? publicationYear, SortField? sortBy, CancellationToken cancellationToken = default)
    {
        var books = await _bookReadRepository.GetAllAsync(cancellationToken);
        var authors = await _authorReadRepository.GetAllAsync(cancellationToken);
        
        var result = MapBooksToDtos(books, authors);

        if (publicationYear.HasValue)
        {
            result = result.Where(book => book.PublicationYear == publicationYear.Value).ToList();
        }

        return SortBooks(result, sortBy ?? SortField.Title);
    }

    public async Task<BookDto> CreateBookAsync(BookDto bookDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(bookDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors.First();
            throw new InvalidBookDataException(firstError.ErrorMessage);
        }
        
        var author = await _authorReadRepository.GetByIdAsync(bookDto.AuthorId, cancellationToken);
        if (author == null)
        {
            throw new AuthorNotFoundException(bookDto.AuthorId);
        }

        var book = new Book(0, bookDto.Title, bookDto.AuthorId, bookDto.PublicationYear);
        var createdBook = await _bookWriteRepository.CreateAsync(book, cancellationToken);

        return new BookDto(createdBook.Id, createdBook.Title, createdBook.AuthorId, author.Name, createdBook.PublicationYear);
    }

    public async Task<AuthorDto> GetAuthorDetailsAsync(int authorId, CancellationToken cancellationToken = default)
    {
        var author = await _authorReadRepository.GetByIdAsync(authorId, cancellationToken);
        if (author == null)
        {
            throw new AuthorNotFoundException(authorId);
        }

        var books = await _bookReadRepository.GetByAuthorIdAsync(authorId, cancellationToken);
        return new AuthorDto(author.Id, author.Name, books);
    }

    private static IReadOnlyList<BookDto> MapBooksToDtos(IReadOnlyList<Book> books, IReadOnlyList<Author> authors)
    {
        var authorLookup = authors.ToDictionary(a => a.Id, a => a);
        return books.Select(book => new BookDto(
            book.Id,
            book.Title,
            book.AuthorId,
            authorLookup.TryGetValue(book.AuthorId, out var author) ? author.Name : "Unknown",
            book.PublicationYear
        )).ToList();
    }

    private static IReadOnlyList<BookDto> SortBooks(IReadOnlyList<BookDto> books, SortField sortField)
    {
        return sortField switch
        {
            SortField.Title => books.OrderBy(b => b.Title).ToList(),
            SortField.TitleDesc => books.OrderByDescending(b => b.Title).ToList(),
            SortField.Year => books.OrderBy(b => b.PublicationYear).ToList(),
            SortField.YearDesc => books.OrderByDescending(b => b.PublicationYear).ToList(),
            SortField.Author => books.OrderBy(b => b.AuthorName).ToList(),
            SortField.AuthorDesc => books.OrderByDescending(b => b.AuthorName).ToList(),
            _ => books.OrderBy(b => b.Title).ToList()
        };
    }
}