namespace Stopwatch.UnitTests;

public class StopwatchTests
{
  [Fact]
  public void Restart()
  {
    var clock = new TestSystemClock()
    {
      UtcNow = new DateTimeOffset(
        2024, 8, 20,
        9, 20, 43,
        TimeSpan.Zero)
    };
    var delay = TimeSpan.FromHours(2);
    var stopwatch = new Stopwatch(clock);

    stopwatch.Start();
    clock.UtcNow += delay;
    stopwatch.Stop();

    stopwatch.Start();
    clock.UtcNow += delay;
    stopwatch.Stop();

    Assert.NotNull(stopwatch.ElapsedTime);
    Assert.Equal(delay, stopwatch.ElapsedTime);
  }

  class TestSystemClock : ISystemClock
  {
    public DateTimeOffset UtcNow { get; set; }
  }
}
