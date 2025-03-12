using System.Text.RegularExpressions;

namespace DoiTools.Net;

public partial class Doi
{
    private string _doi;
    
    private Doi(string doi) => _doi = doi;
    
    [GeneratedRegex(@"^10.\d{4,9}\/[-._;()\/:A-Z0-9<>]+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex ModernDoiRegex();
    
    [GeneratedRegex(@"^10.1002/[^\s]+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex OldDoiRegex();
    
    /// <summary>
    /// Implicitly convert a <see cref="string"/> to a <see cref="Doi"/> object.
    /// </summary>
    /// <param name="doi">The DOI string to convert.</param>
    /// <param name="result">The <see cref="Doi"/> object.</param>
    /// <returns>True if the conversion was successful, otherwise false.</returns>
    public static bool TryParse(string doi, out Doi? result)
    {
        doi = Normalize(doi);
        if (IsValid(doi))
        {
            result = new(doi);
            return true;
        }
        
        result = null;
        return false;
    }
    
    /// <summary>
    /// Parse a DOI string into a <see cref="Doi"/> object.
    /// </summary>
    /// <param name="doi">The DOI string to parse.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when the DOI is invalid.</exception>
    public static Doi Parse(string doi)
    {
        if (TryParse(doi, out Doi? result)) 
            return result!;

        throw new ArgumentException("Invalid DOI", nameof(doi));
    }

    /// <summary>
    /// Normalize a DOI string.
    /// </summary>
    /// <param name="doi">The DOI string to normalize.</param>
    /// <return>The normalized DOI string.</return>
    /// <exception cref="ArgumentException">Thrown when the DOI is invalid.</exception>
    public static string Normalize(string doi)
    {
        if (!doi.StartsWith("http://") && !doi.StartsWith("https://")) 
            return doi;
        
        Uri url;
        try
        {
            url = new(doi);
        }
        catch (UriFormatException)
        {
            throw new ArgumentException("Invalid URL", nameof(doi));
        }
        if (!url.Host.StartsWith("doi.org"))
            throw new ArgumentException("Missing doi.org from URL", nameof(doi));
            
        return url.AbsolutePath.Trim('/');
    }
    
    /// <summary>
    /// Check if a DOI string is valid.
    /// </summary>
    /// <param name="doi">The DOI string to check.</param>
    /// <returns>True if the DOI is valid, otherwise false.</returns>
    public static bool IsValid(string doi) => 
        ModernDoiRegex().IsMatch(doi) || OldDoiRegex().IsMatch(doi);
    
    public override string ToString() => _doi;
}
