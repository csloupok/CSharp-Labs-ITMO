using Backups.Extra.Tools.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class SplitMergeAlgorithm : IMergeAlgorithm
{
    public RestorePoint Merge(RestorePoint firstRestorePoint, RestorePoint secondRestorePoint)
    {
        if (firstRestorePoint is null || secondRestorePoint is null)
            throw new BackupsExtraException("One of the point is null.");

        RestorePoint oldPoint = firstRestorePoint.CreationDate < secondRestorePoint.CreationDate
            ? firstRestorePoint
            : secondRestorePoint;
        RestorePoint newPoint = firstRestorePoint.CreationDate < secondRestorePoint.CreationDate
            ? secondRestorePoint
            : firstRestorePoint;

        List<Storage> storages = new List<Storage>(newPoint.Storages);
        foreach (Storage storage in oldPoint.Storages)
        {
            string storagePath = storage.BackupObjects.First().Path;
            if (!storages.Any(s => s.BackupObjects.First().Path.Equals(storagePath)))
                storages.Add(storage);
        }

        return new RestorePoint(storages, newPoint.CreationDate);
    }
}