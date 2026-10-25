namespace URLShortener.Services;

public interface IUrlShortenerService
{
    ulong Reserve(string originalUrl);
    string?Shorten(string url);
    string? GetOriginalUrlByShortened(string shortenedUrl);
}