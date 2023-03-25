using Backups.Models;

namespace Backups.Extra.Algorithms;

public interface IMergeAlgorithm
{
    RestorePoint Merge(RestorePoint firstRestorePoint, RestorePoint secondRestorePoint);
}