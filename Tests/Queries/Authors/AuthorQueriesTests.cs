using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistences;
using Application.Entities;

namespace Tests.Queries.Authors;

public class AuthorQueriesTests : IDisposable
{
    private readonly AppDbContext _context;

    public AuthorQueriesTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAuthors_WithInMemory_ReturnsAllAuthors()
    {
        // Arrange
        var authors = new List<AuthorEntity>
        {
            new AuthorEntity 
            { 
                Id = 1,
                Name = "Gabriel", 
                LastName = "García", 
                Country = "Colombia", 
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            },
            new AuthorEntity 
            { 
                Id = 2,
                Name = "Jorge Luis", 
                LastName = "Borges", 
                Country = "Argentina", 
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            }
        };

        await _context.AuthorEntities.AddRangeAsync(authors);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.AuthorEntities
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, a => a.Name == "Gabriel");
    }

    [Fact]
    public async Task GetAuthorById_WithInMemory_ReturnsAuthor()
    {
        // Arrange
        var author = new AuthorEntity 
        { 
            Id = 1, 
            Name = "Gabriel",
            LastName = "García",
            Country = "Colombia",
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        await _context.AuthorEntities.AddAsync(author);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.AuthorEntities
            .FirstOrDefaultAsync(a => a.Id == 1 && !a.IsDeleted);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Gabriel", result.Name);
        Assert.Equal("García", result.LastName);
    }

    [Fact]
    public async Task GetAuthorById_NotFound_ReturnsNull()
    {
        // Act
        var result = await _context.AuthorEntities
            .FirstOrDefaultAsync(a => a.Id == 999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAuthors_WithPagination_ReturnsPaginatedResults()
    {
        // Arrange
        var authors = new List<AuthorEntity>
        {
            new AuthorEntity { Id = 1, Name = "Gabriel", LastName = "García", Country = "Colombia", CreatedDate = DateTime.UtcNow, IsDeleted = false },
            new AuthorEntity { Id = 2, Name = "Jorge Luis", LastName = "Borges", Country = "Argentina", CreatedDate = DateTime.UtcNow, IsDeleted = false },
            new AuthorEntity { Id = 3, Name = "Pablo", LastName = "Neruda", Country = "Chile", CreatedDate = DateTime.UtcNow, IsDeleted = false }
        };

        await _context.AuthorEntities.AddRangeAsync(authors);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.AuthorEntities
            .Where(a => !a.IsDeleted)
            .Skip(0)
            .Take(2)
            .ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
