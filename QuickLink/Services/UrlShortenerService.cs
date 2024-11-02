using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using System.Text;

namespace QuickLink.Services
{
    public class UrlShortenerService
    {
        private readonly IDatabase _redisDb;
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string CounterKey = "urlshortener:counter"; // Key for the counter in Redis

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

            // Increment the counter in Redis and get the new value
            var counterValue = await _redisDb.StringIncrementAsync(CounterKey);
            var shortCode = EncodeToBase62(counterValue);

            // Store the original URL in Redis with the short code and an expiration of 30 days
            var isSet = await _redisDb.StringSetAsync(shortCode, originalUrl, TimeSpan.FromDays(30));

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
                throw new KeyNotFoundException($"No original URL found for short code: {shortCode}");
            }

            return originalUrl;
        }

        private string EncodeToBase62(long id)
        {
            var result = new StringBuilder();
            while (id > 0)
            {
                result.Insert(0, Base62Chars[(int)(id % 62)]);
                id /= 62;
            }

            return result.ToString();
        }
    }
}
