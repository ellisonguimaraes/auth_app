using System.Text;

namespace AuthApp.Services.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Decode to base 64
    /// </summary>
    /// <param name="base64EncodedData">Base64 string</param>
    /// <returns>Base64 decoded</returns>
    public static string Base64Decode(this string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    /// <summary>
    /// Encode to base 64
    /// </summary>
    /// <param name="plainText">string to convert</param>
    /// <returns>Base64 encoded</returns>
    public static string Base64Encode(this string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
}
