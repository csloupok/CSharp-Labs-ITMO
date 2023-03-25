using System.Globalization;
using Backups.Models;
using Backups.Tools;
using Backups.Tools.Exceptions;
using Ionic.Zip;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    private string _tempFilesPath;

    public SingleStorageAlgorithm(string tempFilesPath)
    {
        if (string.IsNullOrWhiteSpace(tempFilesPath))
            throw new BackupsException("Path is empty!");
        if (!Directory.Exists(tempFilesPath))
            Directory.CreateDirectory(tempFilesPath);
        _tempFilesPath = tempFilesPath;
    }

    public IReadOnlyList<Storage> CreateBackup(List<BackupObject> backupObjects)
    {
        if (backupObjects.Count == 0)
            throw new BackupsException("No objects to backup!");

        List<Storage> storages = new List<Storage>();
        ZipFile zipFile = new ZipFile();
        string archiveName = "Storage.zip";
        string tempArchivePath = Path.Combine(_tempFilesPath, archiveName);
        Storage storage = new Storage(tempArchivePath);

        foreach (BackupObject backupObject in backupObjects)
        {
            if (backupObject is null)
                throw new BackupsException("Object is null!");
            zipFile.AddFile(backupObject.Path);
            storage.AddObject(backupObject);
        }

        zipFile.Save(tempArchivePath);
        storages.Add(storage);
        return storages;
    }
}