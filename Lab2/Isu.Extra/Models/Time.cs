namespace Isu.Extra.Models;

public class Time
{
    private readonly TimeOnly _startTime;
    private readonly TimeOnly _endTime;

    public Time(string startTime, string endTime)
    {
        _startTime = TimeOnly.Parse(startTime);
        _endTime = TimeOnly.Parse(endTime);
    }

    public TimeOnly StartTime => _startTime;
    public TimeOnly EndTime => _endTime;
}