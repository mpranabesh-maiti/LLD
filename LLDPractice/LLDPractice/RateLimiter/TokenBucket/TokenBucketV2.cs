using System;

namespace LLDPractice.RateLimiter.TokenBucket;

public class TokenBucketV2
{
    private readonly int _bucketSize;
    private int _tokens;
    private DateTime _lastRefillTime;
    TimeSpan _interval;

    public TokenBucketV2(int capacity, TimeSpan interval)
    {
        _bucketSize = _tokens = capacity;
        _interval = interval;
        _lastRefillTime = DateTime.UtcNow;
    }

    public bool UseToken()
    {
        lock (this)
        {
            Refill();

            if (_tokens > 0)
            {
                _tokens--;

                return true;
            }
            return false;
        }
    }

    private void Refill()
    {
        var now = DateTime.UtcNow;

        var elapsed = (now - _lastRefillTime).TotalMilliseconds;
        var tokenToAdd = (int)elapsed / _interval.TotalMilliseconds;
        if (tokenToAdd > 0)
        {
            _tokens = (int)Math.Min(_bucketSize, _tokens + tokenToAdd);
            _lastRefillTime = now;
        }
    }
}
