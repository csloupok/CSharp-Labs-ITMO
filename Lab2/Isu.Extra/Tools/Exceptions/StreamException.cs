namespace Isu.Extra.Tools.Exceptions;

public class StreamException : Exception
{
    private StreamException(string message)
        : base(message) { }

    public static StreamException NullArgument()
    {
        return new StreamException("Stream is null!");
    }

    public static StreamException Full()
    {
        return new StreamException("Stream is full!");
    }

    public static StreamException AlreadyExists()
    {
        return new StreamException("This stream already exists!");
    }
}