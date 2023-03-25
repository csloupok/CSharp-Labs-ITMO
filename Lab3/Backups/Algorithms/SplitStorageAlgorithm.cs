using Backups.Models;
using Backups.Tools;
using Backups.Tools.Exceptions;
using Ionic.Zip;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    private string _tempFilesPath;

    public SplitStorageAlgorithm(string tempFilesPath)
    {
        if (string.IsNullOrWhiteSpace(tempFilesPath))
            throw new BackupsException("Path is empty!");
        if (!Directory.Exists(tempFilesPath))
            Directory.CreateDirectory(tempFilesPath);
        _tempFilesPath = tempFilesPath;
    }

    public string TempFilesPath => _tempFilesPath;

    public IReadOnlyList<Storage> CreateBackup(List<BackupObject> backupObjects)
    {
        if (backupObjects.Count == 0)
            throw new BackupsException("No objects to backup!");

        List<Storage> storages = new List<Storage>();
        foreach (BackupObject backupObject in backupObjects)
        {
            if (backupObject is null)
                throw new BackupsException("Object is null!");
            string fullPath = Path.Combine(_tempFilesPath, Path.GetFileNameWithoutExtension(backupObject.Path) + ".zip");
            Storage storage = new Storage(fullPath);
            ZipFile zipFile = new ZipFile();
            zipFile.TempFileFolder = _tempFilesPath;
            zipFile.AddFile(backupObject.Path);
            zipFile.Save(fullPath);
            storage.AddObject(backupObject);
            storages.Add(storage);
        }

        return storages;
    }
}