namespace Shortee.Interfaces;

public interface IUrlShortenerService
{
    string ShortenUrl(string url, int length = 6);
}
