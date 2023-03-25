namespace Isu.Extra.Tools.Exceptions;

public class GroupException : Exception
{
    private GroupException(string message)
        : base(message) { }

    public static GroupException NullArgument()
    {
        return new GroupException("Group is null!");
    }

    public static GroupException AlreadyExists()
    {
        return new GroupException("Group already exists!");
    }
}