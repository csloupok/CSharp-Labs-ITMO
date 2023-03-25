using System.Globalization;
using Backups.Extra.Tools.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class FilterByDateAlgorithm : IFilterAlgorithm
{
    private DateTime _date;

    public FilterByDateAlgorithm(DateTime date)
    {
        if (!IsValidDateTime(date))
            throw new BackupsExtraException("Date is invalid.");
        _date = date;
    }

    public IReadOnlyList<RestorePoint> Filter(IReadOnlyList<RestorePoint> points)
    {
        if (points is null)
            throw new BackupsExtraException("List of points is null.");
        if (points.Count == 0)
            Console.WriteLine("Warning: List of points is empty.");
        return points.Where(point => point.CreationDate > _date).ToList();
    }

    private bool IsValidDateTime(DateTime dateTime)
    {
        if (dateTime < DateTime.MinValue || dateTime > DateTime.MaxValue)
        {
            return false;
        }

        string dateTimeString = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
        return DateTime.TryParseExact(
            dateTimeString,
            "yyyy-MM-ddTHH:mm:ss.fffffffZ",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal,
            out DateTime _);
    }
}