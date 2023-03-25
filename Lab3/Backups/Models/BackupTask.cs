using Backups.Algorithms;
using Backups.Tools.Exceptions;

namespace Backups.Models;

public class BackupTask
{
    private readonly IStorageAlgorithm _algorithm;
    private Repository _repository;
    private string _name;
    private List<BackupObject> _backupObjects;
    private List<RestorePoint> _restorePoints;

    public BackupTask(string name, IStorageAlgorithm algorithm, Repository repository)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name is empty!");
        _name = name;
        _algorithm = algorithm ?? throw new BackupsException("Algorithm is null!");
        _repository = repository ?? throw new BackupsException("Repository is null!");
        _backupObjects = new List<BackupObject>();
        _restorePoints = new List<RestorePoint>();
    }

    public Repository Repository => _repository;
    public IStorageAlgorithm Algorithm => _algorithm;
    public string Name => _name;
    public IReadOnlyList<BackupObject> BackupObjects => _backupObjects;
    public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

    public void AddBackupObject(BackupObject backupObject)
    {
        if (_backupObjects.Any(x => x.Path == backupObject.Path))
            throw new BackupsException("Backup object already exists!");
        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        if (_backupObjects.All(x => x.Path != backupObject.Path))
            throw new BackupsException("Backup object doesn't exist!");
        _backupObjects.Remove(backupObject);
    }

    public void CreateBackup()
    {
        DateTime date = DateTime.Now;
        IReadOnlyList<Storage> temporaryStorages = _algorithm.CreateBackup(_backupObjects);
        _restorePoints.Add(Repository.NewRestorePoint(temporaryStorages, date, this));
    }

    public void CreateBackup(DateTime date)
    {
        IReadOnlyList<Storage> temporaryStorages = _algorithm.CreateBackup(_backupObjects);
        _restorePoints.Add(Repository.NewRestorePoint(temporaryStorages, date, this));
    }

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupsException("Restore point is null.");
        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupsException("Restore point is null.");
        _restorePoints.Remove(restorePoint);
    }
}