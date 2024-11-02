using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuickLink.Services;

namespace QuickLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly UrlShortenerService _urlShortenerService;

        public UrlShortenerController(UrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        // POST api/urlshortener/shorten
        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return BadRequest("Original URL is required.");
            }

            var shortCode = await _urlShortenerService.ShortenUrlAsync(originalUrl);
            var shortUrl = $"{Request.Scheme}://{Request.Host}/api/urlshortener/{shortCode}";

            return Ok(new
            {
                message = "Your QuickLink has been created!",
                shortUrl
            });
        }

        // GET api/urlshortener/{shortCode}
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var originalUrl = await _urlShortenerService.GetOriginalUrlAsync(shortCode);

            if (originalUrl == null)
                return NotFound("QuickLink not found");

            return Redirect(originalUrl);
        }
    }
}
