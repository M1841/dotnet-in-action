namespace Stopwatch;

public class SystemClock : ISystemClock
{
  public DateTimeOffset UtcNow { get { return DateTimeOffset.UtcNow; } }
}
