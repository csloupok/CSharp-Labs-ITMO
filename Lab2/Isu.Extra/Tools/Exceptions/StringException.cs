namespace Isu.Extra.Tools.Exceptions;

public class StringException : Exception
{
    private StringException(string message)
        : base(message) { }

    public static StringException Empty(string paramName)
    {
        return new StringException($"{paramName} is blank or incorrect input!");
    }
}