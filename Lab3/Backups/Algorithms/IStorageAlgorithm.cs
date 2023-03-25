using Backups.Models;

namespace Backups.Algorithms;

public interface IStorageAlgorithm
{
    IReadOnlyList<Storage> CreateBackup(List<BackupObject> backupObjects);
}