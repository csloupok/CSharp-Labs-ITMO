using Backups.Tools.Exceptions;

namespace Backups.Models;

public class RestorePoint
{
    private const int MinStorageCapacity = 0;
    private DateTime _date;
    private List<Storage> _storages;

    public RestorePoint(List<Storage> storages, DateTime date)
    {
        if (storages.Capacity == MinStorageCapacity)
            throw new BackupsException("Storage list is empty!");
        _date = date;
        _storages = storages;
    }

    public DateTime CreationDate => _date;
    public IReadOnlyList<Storage> Storages => _storages;
}