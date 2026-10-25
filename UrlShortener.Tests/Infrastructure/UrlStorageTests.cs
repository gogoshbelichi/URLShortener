using URLShortener.Domain;
using URLShortener.Infrastructure;

namespace UrlShortener.Tests.Infrastructure;

public class UrlStorageTests
{
    private readonly UrlStorage _sut = new();

    private static Urls CreateUrls(string original = "https://example.com", string shortened = "abc123") =>
        new() { OriginalUrl = original, ShortenedUrl = shortened };

    [Fact]
    public void Insert_FirstItem_ReturnsIdOne()
    {
        var id = _sut.Insert(CreateUrls());

        Assert.Equal(1UL, id);
    }

    [Fact]
    public void Insert_SeveralItems_ReturnsIncrementingIds()
    {
        var id1 = _sut.Insert(CreateUrls(shortened: "a"));
        var id2 = _sut.Insert(CreateUrls(shortened: "b"));

        Assert.Equal(id1 + 1, id2);
    }

    [Fact]
    public void GetOriginalUrlByShortened_ExistingUrl_ReturnsOriginal()
    {
        _sut.Insert(CreateUrls("https://example.com", "abc123"));

        var result = _sut.GetOriginalUrlByShortened("abc123");

        Assert.Equal("https://example.com", result);
    }

    [Fact]
    public void GetOriginalUrlByShortened_UnknownUrl_ReturnsNull()
    { 
        var result = _sut.GetOriginalUrlByShortened("missing");

        Assert.Null(result);
    }

    [Fact]
    public void TryUpdate_ExistingId_UpdatesAndReturnsTrue()
    {
        var id = _sut.Insert(CreateUrls("https://old.com", "abc"));

        var updated = _sut.TryUpdate(id, CreateUrls("https://new.com", "abc"));

        Assert.True(updated);
        Assert.Equal("https://new.com", _sut.GetOriginalUrlByShortened("abc"));
    }

    [Fact]
    public void TryUpdate_UnknownId_ReturnsFalse()
    {
        var updated = _sut.TryUpdate(999, CreateUrls());

        Assert.False(updated);
    }

    [Fact]
    public async Task Insert_ConcurrentCalls_AllIdsUnique()
    {
        var ct = TestContext.Current.CancellationToken;

        var tasks = Enumerable.Range(0, 1000)
            .Select(i => Task.Run(() => _sut.Insert(CreateUrls(shortened: $"s{i}")), ct));

        var ids = await Task.WhenAll(tasks);

        Assert.Equal(1000, ids.Distinct().Count());
        Assert.DoesNotContain(0UL, ids);
    }
}