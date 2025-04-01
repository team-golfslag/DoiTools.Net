// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json;
using DoiTools.Net;

namespace DoiTools.Tests;

public class DoiConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters =
        {
            new DoiConverter(),
        },
    };

    [Theory]
    [InlineData("\"10.1000/xyz123\"", "10.1000/xyz123")]
    [InlineData("\"10.1002/abc456\"", "10.1002/abc456")]
    public void Read_ShouldReturnDoi_WhenJsonIsValid(string json, string expectedDoi)
    {
        // Act
        Doi? result = JsonSerializer.Deserialize<Doi>(json, _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDoi, result.ToString());
    }

    [Theory]
    [InlineData("{\"doi\": null}")]
    [InlineData("\"invalid_doi\"")]
    public void Read_ShouldThrowJsonException_WhenJsonIsInvalid(string json)
    {
        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Doi>(json, _options));
    }

    [Theory]
    [InlineData("10.1000/xyz123", "\"10.1000/xyz123\"")]
    [InlineData("10.1002/abc456", "\"10.1002/abc456\"")]
    public void Write_ShouldReturnJsonString_WhenDoiIsValid(string doi, string expectedJson)
    {
        // Arrange
        Doi? doiInstance = Doi.Parse(doi);

        // Act
        string? result = JsonSerializer.Serialize(doiInstance, _options);

        // Assert
        Assert.Equal(expectedJson, result);
    }
}
