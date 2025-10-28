using BookCatalog.Application.DTOs;
using FluentValidation;

namespace BookCatalog.Application.Validators;

public sealed class BookDtoValidator : AbstractValidator<BookDto>
{
    public BookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must be between 1 and 200 characters");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0)
            .WithMessage("AuthorId must be a positive number");

        RuleFor(x => x.PublicationYear)
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage($"Publication year cannot be in the future. Current year is {DateTime.Now.Year}");
    }
}
