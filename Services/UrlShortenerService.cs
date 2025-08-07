using System.Security.Cryptography;

namespace Shortee.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private const string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string DefaultBaseUrl = "https://short.ee/";

    public string ShortenUrl(string url, int length = 6)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        if (!IsValidUrl(url))
            throw new ArgumentException("Invalid URL format.", nameof(url));

        byte[] hashBytes;
        using (var sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));
        }

        ulong hashNum = BitConverter.ToUInt64(hashBytes, 0);

        var code = Base62Encode(hashNum);

        return $"{DefaultBaseUrl}{(code.Length > length ? code.Substring(0, length) : code)}";
    }

    private string Base62Encode(ulong value)
    {
        var sb = new StringBuilder();
        do
        {
            sb.Insert(0, Base62Chars[(int)(value % 62)]);
            value /= 62;
        } while (value > 0);

        return sb.ToString();
    }

    private bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
