using Isu.Extra.Models;

namespace Isu.Extra.Tools.Exceptions;

public class ScheduleException : Exception
{
    private ScheduleException(string message)
        : base(message) { }

    public static ScheduleException NullArgument()
    {
        return new ScheduleException("Schedule is null!");
    }

    public static ScheduleException NoGroupSchedule()
    {
        return new ScheduleException("Group doesn't have schedule yet!");
    }

    public static ScheduleException NoStreamSchedule()
    {
        return new ScheduleException("Stream doesn't have schedule yet!");
    }

    public static ScheduleException Conflict()
    {
        return new ScheduleException("Schedule is conflicted!");
    }
}
