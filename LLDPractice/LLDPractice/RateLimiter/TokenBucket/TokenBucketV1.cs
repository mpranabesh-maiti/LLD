using System.Collections.Concurrent;
using System.Timers;

namespace LLDPractice.RateLimiter.TokenBucket;

public class TokenBucketV1
{
    private int _maxToken;
    private System.Timers.Timer _timer;
    private BlockingCollection<Token> _tokens;

    public TokenBucketV1(int bucketSize, int intervalMilliSeconds)
    {
        _maxToken = bucketSize;
        _tokens = new BlockingCollection<Token>(bucketSize);
        _timer = new System.Timers.Timer(bucketSize);
        InitializeBucket(bucketSize);
    }

    private void InitializeBucket(int bucketSize)
    {
        foreach (var _ in Enumerable.Range(0, _maxToken))
        {
            _tokens.Add(new Token());
        }

        _timer.AutoReset = true;
        _timer.Enabled = true;
        _timer.Elapsed += TryRefillToken;
    }

    private void TryRefillToken(object? sender, ElapsedEventArgs e)
    {
        foreach (var _ in Enumerable.Range(0, _maxToken - _tokens.Count))
        {
            _tokens.Add(new Token());
        }
    }

    public bool UseToken()
    {
        return _tokens.TryTake(out Token _) ? true : throw new RateLimiterException();
    }
}

public record Token;
