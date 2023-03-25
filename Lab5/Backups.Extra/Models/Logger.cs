using Backups.Extra.Tools.Exceptions;

namespace Backups.Extra.Models;

public class Logger
{
    private static string? _mode;
    private static string? _pathToFile;

    public static void Log(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new BackupsExtraException("Log is empty.");
        if (_mode == "Console")
        {
            Console.WriteLine(text);
        }
        else
        {
            File.AppendText(text);
        }
    }

    public static void SetConsoleMode()
    {
        _mode = "Console";
    }

    public static void SetFileMode(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsExtraException("Path is empty.");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        _mode = "File";
        _pathToFile = path;
    }
}