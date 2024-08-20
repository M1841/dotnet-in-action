namespace Stopwatch;

public interface ISystemClock
{
  DateTimeOffset UtcNow { get; }
}
