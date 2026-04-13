using Xunit;
using Moq;
using Core.Domains.Authors;
using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.DTOs.Authors;

namespace Tests.Domains.Authors;

public class CreateAuthorDomainTests
{
    private readonly Mock<IAuthorRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateAuthorDomain _domain;

    public CreateAuthorDomainTests()
    {
        _mockRepository = new Mock<IAuthorRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _domain = new CreateAuthorDomain(_mockRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsAuthorResponse()
    {
        // Arrange
        var request = new CreateAuthorRequest
        {
            Name = "Gabriel",
            LastName = "García",
            BirthDate = new DateTime(1927, 3, 6),
            Country = "Colombia",
            Biography = "Escritor famoso"
        };

        _mockUnitOfWork.Setup(u => u.BeginAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _domain.CreateAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Gabriel", result.Name);
        Assert.Equal("García", result.LastName);
        Assert.Equal("Colombia", result.Country);
    }

    [Fact]
    public async Task CreateAsync_WithoutBiography_ReturnsAuthorResponse()
    {
        // Arrange - Test that Biography can be null
        var request = new CreateAuthorRequest
        {
            Name = "Jorge Luis",
            LastName = "Borges",
            BirthDate = new DateTime(1899, 8, 24),
            Country = "Argentina",
            Biography = string.Empty  // Biography can be empty
        };

        _mockUnitOfWork.Setup(u => u.BeginAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _domain.CreateAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jorge Luis", result.Name);
        Assert.Empty(result.Biography);
    }

    [Fact]
    public async Task CreateAsync_CallsBeginAndCommit()
    {
        // Arrange
        var request = new CreateAuthorRequest
        {
            Name = "Test",
            LastName = "Author",
            BirthDate = DateTime.Now,
            Country = "Test",
            Biography = "Test"
        };

        _mockUnitOfWork.Setup(u => u.BeginAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _domain.CreateAsync(request, CancellationToken.None);

        // Assert
        _mockUnitOfWork.Verify(u => u.BeginAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
