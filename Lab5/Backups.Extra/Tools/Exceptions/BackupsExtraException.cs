namespace Backups.Extra.Tools.Exceptions;

public class BackupsExtraException : Exception
{
    public BackupsExtraException()
    {
    }

    public BackupsExtraException(string message)
        : base(message)
    {
    }

    public BackupsExtraException(string message, Exception inner)
        : base(message, inner)
    {
    }
}