using Backups.Tools.Exceptions;

namespace Backups.Models;

public class Repository
{
    private string _location;

    public Repository(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new BackupsException("Path is empty!");
        if (!Directory.Exists(location))
            Directory.CreateDirectory(location);
        _location = location;
    }

    public string Location => _location;

    public RestorePoint NewRestorePoint(IReadOnlyList<Storage> tempStorages, DateTime date, BackupTask backupTask)
    {
        if (tempStorages.Count == 0)
            throw new BackupsException("List of storage is empty!");
        if (backupTask is null)
            throw new BackupsException("Backup task is null!");
        SaveRestorePoint(tempStorages, backupTask);
        List<Storage> storages = new List<Storage>();

        foreach (Storage tempStorage in tempStorages)
        {
            if (tempStorage is null)
                throw new BackupsException("Storage is null!");
            string fileName = Path.GetFileName(tempStorage.PathToFile);
            Storage storage = new Storage(fileName);
            storage.AddObjects(tempStorage.BackupObjects);
            storages.Add(storage);
        }

        RestorePoint restorePoint = new RestorePoint(storages, date);
        ClearTempStorages(tempStorages);
        return restorePoint;
    }

    private void SaveRestorePoint(IReadOnlyList<Storage> storages, BackupTask backupTask)
    {
        if (storages.Count == 0)
            throw new BackupsException("List of storage is empty!");
        if (backupTask is null)
            throw new BackupsException("Backup task is null!");
        string subDirectory = _location + $"/{backupTask.Name}" + $"/RestorePoint_{backupTask.RestorePoints.Count}";
        if (!Directory.Exists(subDirectory))
            Directory.CreateDirectory(subDirectory);
        foreach (Storage storage in storages)
        {
            string path = Path.Combine(_location, subDirectory, Path.GetFileName(storage.PathToFile));
            File.Copy(storage.PathToFile, path);
        }
    }

    private void ClearTempStorages(IReadOnlyList<Storage> storages)
    {
        if (storages.Count == 0)
            throw new BackupsException("List of storage is empty!");
        foreach (Storage storage in storages)
        {
            File.Delete(storage.PathToFile);
        }
    }
}