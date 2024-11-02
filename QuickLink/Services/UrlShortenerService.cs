using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace QuickLink.Services
{
    public class UrlShortenerService
    {
        private readonly IDatabase _redisDb;

        public UrlShortenerService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<string> ShortenUrlAsync(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                throw new ArgumentException("Original URL cannot be null or empty.", nameof(originalUrl));
            }

            var shortCode = GenerateShortCode();

            // Store the original URL in Redis with the short code
            // Use a RedisValue to handle the potential null case
            var isSet = await _redisDb.StringSetAsync(shortCode, originalUrl);

            if (!isSet)
            {
                throw new Exception("Could not store the URL in Redis.");
            }

            return shortCode;
        }

        public async Task<string> GetOriginalUrlAsync(string shortCode)
        {
            // Retrieve the original URL from Redis
            var originalUrl = await _redisDb.StringGetAsync(shortCode);

            // Check if the original URL is null or empty
            if (originalUrl.IsNullOrEmpty)
            {
                return null; // or throw an exception, depending on your needs
            }

            return originalUrl;
        }

        private string GenerateShortCode()
        {
            // Generate a simple short code
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }
    }
}
