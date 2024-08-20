namespace Stopwatch;

public class Stopwatch(ISystemClock? systemClock = null)
{
  public void Start()
  {
    _startTime = _clock.UtcNow;
    _stopTime = null;
  }
  public void Stop()
  {
    if (_startTime != null) { _stopTime = _clock.UtcNow; }
  }
  public TimeSpan? ElapsedTime
  {
    get
    {
      if (_startTime != null && _stopTime != null)
      {
        return _stopTime - _startTime;
      }
      return null;
    }
  }

  private DateTimeOffset? _startTime;
  private DateTimeOffset? _stopTime;
  private readonly ISystemClock _clock = systemClock ?? DefaultClock;
  private static readonly ISystemClock DefaultClock = new SystemClock();
}
