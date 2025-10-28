-- =============================================
-- Book Catalog Database - Script (If backup won't work)
-- =============================================
-- CREATE Tables
CREATE TABLE [dbo].[Authors] (
[Id]   INT            IDENTITY (1, 1) NOT NULL,
[Name] NVARCHAR (100) NOT NULL,
CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Books] (
[Id]              INT            IDENTITY (1, 1) NOT NULL,
[Title]           NVARCHAR (200) NOT NULL,
[AuthorId]        INT            NOT NULL,
[PublicationYear] INT            NOT NULL,
CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([Id] ASC),
CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Authors] ([Id])
);
GO
CREATE NONCLUSTERED INDEX [IX_Books_AuthorId]
ON [dbo].[Books]([AuthorId] ASC);
-- INSERT Sample Data
INSERT INTO [dbo].[Authors] ([Name])
VALUES
('J.K. Rowling'),
('Stephen King'),
('George Orwell'),
('Agatha Christie'),
('J.R.R. Tolkien'),
('Ernest Hemingway'),
('Mark Twain'),
('Jane Austen');
INSERT INTO [dbo].[Books] ([Title], [AuthorId], [PublicationYear])
VALUES
('Harry Potter and the Philosopher''s Stone', 1, 1997),
('Harry Potter and the Chamber of Secrets', 1, 1998),
('Harry Potter and the Prisoner of Azkaban', 1, 1999),
('The Shining', 2, 1977),
('IT', 2, 1986),
('Pet Sematary', 2, 1983),
('1984', 3, 1949),
('Animal Farm', 3, 1945),
('Murder on the Orient Express', 4, 1934),
('And Then There Were None', 4, 1939),
('The Hobbit', 5, 1937),
('The Lord of the Rings', 5, 1954),
('The Old Man and the Sea', 6, 1952),
('For Whom the Bell Tolls', 6, 1940),
('The Adventures of Tom Sawyer', 7, 1876),
('Adventures of Huckleberry Finn', 7, 1884),
('Pride and Prejudice', 8, 1813),
('Emma', 8, 1815);
-- UPDATE Operations
-- Update a single book's publication year
UPDATE [dbo].[Books]
SET [PublicationYear] = 1950
WHERE [Title] = '1984';
-- Update author's name
UPDATE [dbo].[Authors]
SET [Name] = 'J.K. Rowling (Joanne Rowling)'
WHERE [Id] = 1;
-- Update multiple books for an author
UPDATE [dbo].[Books]
SET [Title] = 'LOTR: ' + [Title]
WHERE [AuthorId] = 5 AND [Title] LIKE '%Lord%';
-- DELETE Operations
-- Delete a specific book
DELETE FROM [dbo].[Books]
WHERE [Title] = 'Pet Sematary';
-- Delete books published before 1850
DELETE FROM [dbo].[Books]
WHERE [PublicationYear] < 1850;
-- SELECT Operations
-- 1. Select all authors
SELECT * FROM [dbo].[Authors];
-- 2. Select all books
SELECT * FROM [dbo].[Books];
-- 3. Select books with author names using JOIN
SELECT
b.[Title],
a.[Name] AS [Author],
b.[PublicationYear]
FROM [dbo].[Books] b
INNER JOIN [dbo].[Authors] a ON b.[AuthorId] = a.[Id]
ORDER BY b.[PublicationYear] DESC;
-- 4. Select books published after 1950
SELECT
b.[Title],
a.[Name] AS [Author],
b.[PublicationYear]
FROM [dbo].[Books] b
INNER JOIN [dbo].[Authors] a ON b.[AuthorId] = a.[Id]
WHERE b.[PublicationYear] > 1950;
-- 5. Count books per author using GROUP BY
SELECT
a.[Name] AS [Author],
COUNT(b.[Id]) AS [BookCount]
FROM [dbo].[Authors] a
LEFT JOIN [dbo].[Books] b ON a.[Id] = b.[AuthorId]
GROUP BY a.[Id], a.[Name]
ORDER BY [BookCount] DESC;
-- 6. Find authors with more than 2 books using HAVING
SELECT
a.[Name] AS [Author],
COUNT(b.[Id]) AS [NumberOfBooks]
FROM [dbo].[Authors] a
INNER JOIN [dbo].[Books] b ON a.[Id] = b.[AuthorId]
GROUP BY a.[Id], a.[Name]
HAVING COUNT(b.[Id]) > 2;
-- 7. Find the oldest and newest books
SELECT TOP 1
b.[Title],
a.[Name] AS [Author],
b.[PublicationYear]
FROM [dbo].[Books] b
INNER JOIN [dbo].[Authors] a ON b.[AuthorId] = a.[Id]
ORDER BY b.[PublicationYear] ASC;
SELECT TOP 1
b.[Title],
a.[Name] AS [Author],
b.[PublicationYear]
FROM [dbo].[Books] b
INNER JOIN [dbo].[Authors] a ON b.[AuthorId] = a.[Id]
ORDER BY b.[PublicationYear] DESC;
-- 8. Books by decade
SELECT
(b.[PublicationYear] / 10) * 10 AS [Decade],
COUNT(*) AS [BooksCount]
FROM [dbo].[Books] b
GROUP BY (b.[PublicationYear] / 10) * 10
ORDER BY [Decade];
-- 9. Search books by partial title
SELECT
b.[Title],
a.[Name] AS [Author],
b.[PublicationYear]
FROM [dbo].[Books] b
INNER JOIN [dbo].[Authors] a ON b.[AuthorId] = a.[Id]
WHERE b.[Title] LIKE '%Harry%';
-- 10. Authors and their earliest/latest publications
SELECT
a.[Name] AS [Author],
MIN(b.[PublicationYear]) AS [FirstPublication],
MAX(b.[PublicationYear]) AS [LastPublication],
COUNT(b.[Id]) AS [TotalBooks]
FROM [dbo].[Authors] a
INNER JOIN [dbo].[Books] b ON a.[Id] = b.[AuthorId]
GROUP BY a.[Id], a.[Name]
ORDER BY a.[Name];