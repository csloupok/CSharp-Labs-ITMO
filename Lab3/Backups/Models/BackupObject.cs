using Backups.Tools.Exceptions;

namespace Backups.Models;

public class BackupObject
{
    private string _path;

    public BackupObject(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Path to file is empty!");
        _path = path;
    }

    public string Path => _path;
}