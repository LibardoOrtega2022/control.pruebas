using Xunit;
using Moq;
using Core.Domains.Books;
using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.Abstractions.Books;
using Application.DTOs.Books;

namespace Tests.Domains.Books;

public class CreateBookDomainTests
{
    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly Mock<IAuthorRepository> _mockAuthorRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateBookDomain _domain;

    public CreateBookDomainTests()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockAuthorRepository = new Mock<IAuthorRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _domain = new CreateBookDomain(_mockBookRepository.Object, _mockAuthorRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsBookResponse()
    {
        // Arrange
        var author = new Application.Entities.AuthorEntity 
        { 
            Id = 1, 
            Name = "Gabriel",
            LastName = "García Márquez"
        };

        var request = new CreateBookRequest
        {
            Title = "100 Años de Soledad",
            NumberOfPages = 417,
            AuthorId = 1,
            Genre = "Realismo Mágico",
            PublishedDate = new DateTime(1967, 5, 30),
            ISBN = "978-0060883287"
        };

        _mockAuthorRepository.Setup(a => a.GetAuthorByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _mockUnitOfWork.Setup(u => u.BeginAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _domain.CreateAsync(request, null, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("100 Años de Soledad", result.Title);
        Assert.Equal(417, result.NumberOfPages);
        Assert.Equal("Realismo Mágico", result.Genre);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidAuthorId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var request = new CreateBookRequest
        {
            Title = "Test Book",
            NumberOfPages = 100,
            AuthorId = 999, // No existe
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            ISBN = "123"
        };

        _mockAuthorRepository.Setup(a => a.GetAuthorByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Application.Entities.AuthorEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _domain.CreateAsync(request, null, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_WithFutureDate_ThrowsArgumentException()
    {
        // Arrange - Domain validates PublishedDate cannot be in future
        var author = new Application.Entities.AuthorEntity 
        { 
            Id = 1,
            Name = "Gabriel",
            LastName = "García Márquez"
        };

        var request = new CreateBookRequest
        {
            Title = "Future Book",
            NumberOfPages = 100,
            AuthorId = 1,
            Genre = "Fiction",
            PublishedDate = DateTime.UtcNow.AddDays(1), // Future date
            ISBN = "123"
        };

        _mockAuthorRepository.Setup(a => a.GetAuthorByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _domain.CreateAsync(request, null, CancellationToken.None));
    }
}
