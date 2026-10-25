using System.Collections.Concurrent;
using URLShortener.Domain;

namespace URLShortener.Infrastructure;

internal sealed class UrlStorage : IUrlStorage
{
    private ulong _counter;
    private readonly ConcurrentDictionary<ulong, Urls> _storage = new();
    
    public string? GetOriginalUrlByShortened(string shortenedUrl)
    { 
        var x = _storage.FirstOrDefault(x => x.Value.ShortenedUrl == shortenedUrl);

        Console.WriteLine($"Got from storage - {x}");
        
        if (x.Value == null)
        {
            return null;
        }
        
        return x.Value.OriginalUrl;
    }

    public ulong Insert(Urls urls)
    {
        var id = Interlocked.Increment(ref _counter);

        var success = _storage.TryAdd(id, urls);

        if (success) return id;

        return 0;

    }

    public bool TryUpdate(ulong id, Urls urls)
    {
        var hasValue = _storage.TryGetValue(id, out var old);

        if (hasValue && old != null)
        {
            return _storage.TryUpdate(id, urls, old);
        }

        return hasValue;
    }
}