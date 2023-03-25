using Isu.Extra.Models;

namespace Isu.Extra.Tools.Exceptions;

public class LessonException : Exception
{
    private LessonException(string message)
        : base(message) { }

    public static LessonException NullArgument(string paramName)
    {
        throw new LessonException($"{paramName} is null!");
    }

    public static LessonException Conflict()
    {
        throw new LessonException("Lesson conflicts with schedule!");
    }

    public static LessonException NoLesson()
    {
        throw new LessonException("No such lesson!");
    }
}