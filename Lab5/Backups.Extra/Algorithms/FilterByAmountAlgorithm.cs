using Backups.Extra.Tools.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class FilterByAmountAlgorithm : IFilterAlgorithm
{
    private const int MinAmountOfPoints = 0;
    private int _amount;

    public FilterByAmountAlgorithm(int amount)
    {
        if (amount <= MinAmountOfPoints)
            throw new BackupsExtraException("Incorrect number of points.");
        _amount = amount;
    }

    public IReadOnlyList<RestorePoint> Filter(IReadOnlyList<RestorePoint> points)
    {
        if (points is null)
            throw new BackupsExtraException("List of points is null.");
        if (points.Count == MinAmountOfPoints)
            Console.WriteLine("Warning: List of points is empty.");
        return points.Take(_amount).ToList();
    }
}