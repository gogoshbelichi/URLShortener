using Microsoft.Extensions.Options;
using URLShortener.Domain;
using URLShortener.Infrastructure;
using URLShortener.Options;

namespace URLShortener.Services;

internal class UrlShortenerService : IUrlShortenerService
{
    private readonly IUrlStorage _urlStorage;
    private readonly IBase62Converter _base62Converter;
    private readonly IOptions<UrlShortenerOptions> _options;

    public UrlShortenerService(IUrlStorage urlStorage, IBase62Converter base62Converter, IOptions<UrlShortenerOptions> options)
    {
        _urlStorage = urlStorage;
        _base62Converter = base62Converter;
        _options = options;
    }
    
    public ulong Reserve(string originalUrl)
    {
        var result = _urlStorage.Insert(new Urls
        {
            OriginalUrl = originalUrl,
            ShortenedUrl = string.Empty,
        });
        
        return result;
    }

    public string? Shorten(string url)
    {
        var id = Reserve(url);
        
        var base62String = _base62Converter.ToBase62String(id);
        
        var result = _options.Value.ShortenedUrlPattern + base62String;
        
        var success = _urlStorage.TryUpdate(id, new Urls
        {
            OriginalUrl = url,
            ShortenedUrl = result,
        });
        
        return success ? result : null;
    }

    public string? GetOriginalUrlByShortened(string shortenedUrl)
    {
        return _urlStorage.GetOriginalUrlByShortened(_options.Value.ShortenedUrlPattern + shortenedUrl);
    }
}