using Backups.Models;

namespace Backups.Extra.Algorithms;

public interface IFilterAlgorithm
{
    IReadOnlyList<RestorePoint> Filter(IReadOnlyList<RestorePoint> points);
}