using Microsoft.AspNetCore.Mvc;
using URLShortener.Services;

namespace URLShortener.Controllers;

[ApiController]
[Route("urlshortener")]
public sealed class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService;

    public UrlShortenerController(IUrlShortenerService urlShortenerService)
    {
        _urlShortenerService = urlShortenerService;
    }
    
    [HttpGet("/{shortenedUrl}")]
    public IActionResult RedirectTo(string shortenedUrl)
    {
        var result = _urlShortenerService.GetOriginalUrlByShortened(shortenedUrl);

        if (result == null)
        {
            return NotFound();
        }
        
        return Redirect(result);
    }

    [HttpPost("create")]
    public IActionResult Create([FromQuery] string url)
    {
        // TODO: Move to validato
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return BadRequest("Url must be an absolute http or https address.");
        }

        var result = _urlShortenerService.Shorten(uri.ToString());

        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
}