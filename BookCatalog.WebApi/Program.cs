using BookCatalog.Application.DTOs;
using BookCatalog.Application.Interfaces;
using BookCatalog.Application.Services;
using BookCatalog.Application.Validators;
using BookCatalog.Infrastructure.DbContext;
using BookCatalog.Infrastructure.Repositories.Read;
using BookCatalog.Infrastructure.Repositories.Write;
using BookCatalog.WebApi.ExceptionHandlers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Book Catalog API",
            Version = "v1"
        });
    });
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IBookReadRepository, BookReadRepository>();
builder.Services.AddScoped<IBookWriteRepository, BookWriteRepository>();
builder.Services.AddScoped<IAuthorReadRepository, AuthorReadRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IValidator<BookDto>, BookDtoValidator>();

var app = builder.Build();

// Configure exception handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        
        if (exception != null)
        {
            await GlobalExceptionHandler.HandleExceptionAsync(context, exception, logger);
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();