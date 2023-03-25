namespace Isu.Extra.Tools.Exceptions;

public class StudentException : Exception
{
    private StudentException(string message)
        : base(message) { }

    public static StudentException NullArgument()
    {
        return new StudentException("Student is null!");
    }
}