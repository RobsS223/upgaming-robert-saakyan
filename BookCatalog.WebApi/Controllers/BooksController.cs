using BookCatalog.Application.DTOs;
using BookCatalog.Application.Enums;
using BookCatalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.WebApi.Controllers;

[ApiController]
public sealed class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("api/books")]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooks(CancellationToken cancellationToken = default)
    {
        return Ok(await _bookService.GetAllBooksAsync(cancellationToken));
    }

    [HttpGet("api/books/filter")]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooks(
        [FromQuery] int? publicationYear, 
        [FromQuery] string? sortBy, 
        CancellationToken cancellationToken = default)
    {
        var sortField = ParseSortField(sortBy);
        if (sortField == null && !string.IsNullOrWhiteSpace(sortBy))
        {
            return BadRequest(new { error = $"Invalid sort field '{sortBy}'. Valid options are: {string.Join(", ", Enum.GetNames<SortField>())}" });
        }

        return Ok(await _bookService.GetAllBooksAsync(publicationYear, sortField, cancellationToken));
    }

    [HttpGet("api/authors/{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthorDetails(int id, CancellationToken cancellationToken = default)
    {
        return Ok(await _bookService.GetAuthorDetailsAsync(id, cancellationToken));
    }

    [HttpPost("api/books")]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto bookDto, CancellationToken cancellationToken = default)
    {
        var book = await _bookService.CreateBookAsync(bookDto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, book);
    }

    private static SortField? ParseSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return null;

        return Enum.TryParse<SortField>(sortBy, true, out var parsedSortField) ? parsedSortField : null;
    }
}