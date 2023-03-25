namespace Isu.Extra.Tools.Exceptions;

public class CourseException : Exception
{
    private CourseException(string message)
        : base(message) { }

    public static CourseException NullArgument()
    {
        return new CourseException("Course is null!");
    }

    public static CourseException SameFacultyEnroll()
    {
        return new CourseException("Can't enroll to same faculty course!");
    }

    public static CourseException SameCourseEnroll()
    {
        return new CourseException("Can't enroll to same course!");
    }

    public static CourseException TooManyCourse()
    {
        return new CourseException("Can't enroll to more than 2 extra courses!");
    }

    public static CourseException NotEnrolled()
    {
        return new CourseException("Not enrolled to this course!");
    }

    public static CourseException AlreadyExists()
    {
        return new CourseException("Course from this faculty already exists!");
    }

    public static CourseException NoStreams()
    {
        return new CourseException("Course doesn't have any streams!");
    }

    public static CourseException TooManyStreams(int number)
    {
        return new CourseException($"Course can't have more than {number} streams!");
    }
}
