namespace LLDPractice.RateLimiter.TokenBucket;

public class RateLimiterException : Exception
{
    public RateLimiterException() { }

    public RateLimiterException(string message) : base(message) { }
}
