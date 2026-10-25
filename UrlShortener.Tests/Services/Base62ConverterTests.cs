using URLShortener.Services;

namespace UrlShortener.Tests.Infrastructure;

public class Base62ConverterTests
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private readonly Base62Converter _sut = new();

    public static TheoryData<ulong, string> KnownValues => new()
    {
        { 0, "0" },
        { 1, "1" },
        { 9, "9" },
        { 10, "a" },
        { 35, "z" },
        { 36, "A" },
        { 61, "Z" },
        { 62, "10" },
        { 63, "11" },
        { 125, "21" },
        { 3843, "ZZ" },
        { 3844, "100" },
        { ulong.MaxValue, "lYGhA16ahyf" },
    };

    [Theory]
    [MemberData(nameof(KnownValues))]
    public void ToBase62String_KnownValue_ReturnsExpected(ulong id, string expected)
    {
        var result = _sut.ToBase62String(id);

        Assert.Equal(expected, result);
    }

    public static TheoryData<int> Powers => new() { 1, 2, 3, 5, 8, 10 };

    [Theory]
    [MemberData(nameof(Powers))]
    public void ToBase62String_PowerOf62_ReturnsOneFollowedByZeros(int power)
    {
        ulong id = 1;
        for (var i = 0; i < power; i++) id *= 62;

        var result = _sut.ToBase62String(id);

        Assert.Equal("1" + new string('0', power), result);
    }

    public static TheoryData<ulong> RoundTripValues => new()
    {
        0, 1, 61, 62, 1_000_000, 123_456_789_012, ulong.MaxValue - 1, ulong.MaxValue,
    };

    [Theory]
    [MemberData(nameof(RoundTripValues))]
    public void ToBase62String_DecodedBack_ReturnsOriginalId(ulong id)
    {
        var encoded = _sut.ToBase62String(id);

        Assert.Equal(id, _sut.ToULong(encoded));
    }

    [Fact]
    public void ToBase62String_RangeOfIds_ProducesUniqueStrings()
    {
        const int count = 100_000;

        var results = Enumerable.Range(0, count)
            .Select(i => _sut.ToBase62String((ulong)i))
            .ToHashSet();

        Assert.Equal(count, results.Count);
    }

    [Fact]
    public void ToBase62String_RangeOfIds_UsesOnlyAlphabetAndNoLeadingZeros()
    {
        var results = Enumerable.Range(1, 10_000)
            .Select(i => _sut.ToBase62String((ulong)i));

        Assert.All(results, s =>
        {
            Assert.All(s, c => Assert.Contains(c, Alphabet));
            Assert.NotEqual('0', s[0]);
        });
    }

    [Fact]
    public void ToBase62String_MaxValue_HasElevenCharacters()
    {
        Assert.Equal(11, _sut.ToBase62String(ulong.MaxValue).Length);
    }
}