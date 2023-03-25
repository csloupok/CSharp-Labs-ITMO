using Backups.Extra.Tools.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class SingleMergeAlgorithm : IMergeAlgorithm
{
    public RestorePoint Merge(RestorePoint firstRestorePoint, RestorePoint secondRestorePoint)
    {
        if (firstRestorePoint is null || secondRestorePoint is null)
            throw new BackupsExtraException("One of the point is null.");

        RestorePoint newPoint = firstRestorePoint.CreationDate < secondRestorePoint.CreationDate ? secondRestorePoint : firstRestorePoint;
        return newPoint;
    }
}