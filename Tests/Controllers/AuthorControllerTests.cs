using Xunit;
using System.Net;
using System.Text.Json;
using Application.DTOs.Authors;

namespace Tests.Controllers;

public class AuthorControllerTests
{
    [Fact]
    public void SerializeAuthorRequest_Works()
    {
        // Arrange
        var request = new CreateAuthorRequest
        {
            Name = "Test",
            LastName = "Author",
            BirthDate = DateTime.Now,
            Country = "Colombia",
            Biography = "Test Biography"
        };

        // Act
        var json = JsonSerializer.Serialize(request);

        // Assert
        Assert.NotNull(json);
        Assert.Contains("Test", json);
        Assert.Contains("Author", json);
    }

    [Fact]
    public void DeserializeAuthorResponse_Works()
    {
        // Arrange
        var json = @"{
            ""Id"": 1,
            ""Name"": ""Gabriel"",
            ""LastName"": ""García"",
            ""BirthDate"": ""1927-03-06T00:00:00Z"",
            ""Country"": ""Colombia"",
            ""Biography"": ""Author"",
            ""CreatedDate"": ""2026-04-13T12:00:00Z"",
            ""UpdatedDate"": null,
            ""BookCount"": 5
        }";

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Act
        var response = JsonSerializer.Deserialize<AuthorResponse>(json, options);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(1, response.Id);
        Assert.Equal("Gabriel", response.Name);
        Assert.Equal("García", response.LastName);
    }
}
