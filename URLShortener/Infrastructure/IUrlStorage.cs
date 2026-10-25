using URLShortener.Domain;

namespace URLShortener.Infrastructure;

internal interface IUrlStorage
{
    string? GetOriginalUrlByShortened(string shortenedUrl);
    public ulong Insert(Urls urls);
    public bool TryUpdate(ulong id, Urls urls);
}