using Backups.Tools.Exceptions;

namespace Backups.Models;

public class Storage
{
    private const int MinAmountOfBackupObjects = 0;
    private string _pathToFile;
    private List<BackupObject> _backupObjects;

    public Storage(string pathToFile)
    {
        if (string.IsNullOrWhiteSpace(pathToFile))
            throw new BackupsException("Path to file is empty!");
        _pathToFile = pathToFile;
        _backupObjects = new List<BackupObject>();
    }

    public string PathToFile => _pathToFile;
    public IReadOnlyList<BackupObject> BackupObjects => _backupObjects;

    public void AddObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupsException("Object is null!");
        _backupObjects.Add(backupObject);
    }

    public void AddObjects(IReadOnlyList<BackupObject> backupObjects)
    {
        if (backupObjects.Count == MinAmountOfBackupObjects)
            throw new BackupsException("List is empty!");
        foreach (BackupObject backupObject in backupObjects)
        {
            if (backupObject is null)
                throw new BackupsException("Object is null!");
            _backupObjects.Add(backupObject);
        }
    }
}