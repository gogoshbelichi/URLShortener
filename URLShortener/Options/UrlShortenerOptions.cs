namespace URLShortener.Options;

internal class UrlShortenerOptions
{
    // For appsettings/external provider
    internal const string SectionName = "UrlShortener";

    internal required string ShortenedUrlPattern { get; set; } = "https://localhost:7012/";
}