using DoiTools.Net;

namespace DoiTools.Tests;

public class DoiTests
{
    [Theory]
    [InlineData("10.1000/xyz123", true)]
    [InlineData("10.1002/xyz123", true)]
    [InlineData("invalid_doi", false)]
    public void IsValid_ShouldReturnExpectedResult(string doi, bool expected)
    {
        // Act
        bool result = Doi.IsValid(doi);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("10.1000/xyz123")]
    [InlineData("10.1002/xyz123")]
    public void TryParse_ShouldReturnTrueAndSetResult_WhenDoiIsValid(string doi)
    {
        // Act
        bool result = Doi.TryParse(doi, out Doi? parsedDoi);

        // Assert
        Assert.True(result);
        Assert.NotNull(parsedDoi);
        Assert.Equal(doi, parsedDoi.ToString());
    }

    [Theory]
    [InlineData("invalid_doi")]
    public void TryParse_ShouldReturnFalseAndSetResultToNull_WhenDoiIsInvalid(string doi)
    {
        // Act
        bool result = Doi.TryParse(doi, out Doi? parsedDoi);

        // Assert
        Assert.False(result);
        Assert.Null(parsedDoi);
    }

    [Theory]
    [InlineData("10.1000/xyz123")]
    [InlineData("10.1002/xyz123")]
    public void Parse_ShouldReturnDoi_WhenDoiIsValid(string doi)
    {
        // Act
        Doi result = Doi.Parse(doi);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(doi, result.ToString());
    }

    [Theory]
    [InlineData("invalid_doi")]
    public void Parse_ShouldThrowArgumentException_WhenDoiIsInvalid(string doi)
    {
        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => Doi.Parse(doi));
        Assert.Equal("Invalid DOI (Parameter 'doi')", exception.Message);
    }

    [Theory]
    [InlineData("http://doi.org/10.1000/xyz123", "10.1000/xyz123")]
    [InlineData("https://doi.org/10.1000/xyz123", "10.1000/xyz123")]
    [InlineData("10.1000/xyz123", "10.1000/xyz123")]
    public void Normalize_ShouldReturnNormalizedDoi_WhenDoiIsValid(string doi, string expected)
    {
        // Act
        string result = Doi.Normalize(doi);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("http://invalid.org/10.1000/xyz123")]
    [InlineData("https://10.1000/xyz123")]
    public void Normalize_ShouldThrowArgumentException_WhenDoiIsInvalidUrl(string doi)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Doi.Normalize(doi));
    }
}