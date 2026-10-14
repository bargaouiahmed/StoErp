namespace StoErp.Shared.Infrastructure.Messaging.Outbox;

public static class OutboxRetryPolicy
{
    public const int MaxRetryAttempts = 5;

    public static bool ShouldDeadLetter(int attemptCount)
    {
        return attemptCount >= MaxRetryAttempts;
    }
    public static DateTimeOffset CalculateNextAttemptAt(
        int nextAttemptCount,
        DateTimeOffset now)
    {
        if (nextAttemptCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(nextAttemptCount),
                "Attempt count must be greater than zero.");
        }

        var delaySeconds = Math.Min(
            300,
            Math.Pow(2, nextAttemptCount));

        return now.AddSeconds(delaySeconds);
    }
}
