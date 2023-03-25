using System.Text.Json;
using Backups.Algorithms;
using Backups.Extra.Algorithms;
using Backups.Extra.Tools.Exceptions;
using Backups.Models;
using Ionic.Zip;

namespace Backups.Extra.Models;

public class BackupExtraTask : BackupTask
{
    private const int MinNumberOfRestorePoint = 0;
    private IMergeAlgorithm _mergeAlgorithm;
    private List<IFilterAlgorithm> _filterAlgorithms;

    public BackupExtraTask(
        string name,
        IStorageAlgorithm algorithm,
        Repository repository,
        IMergeAlgorithm mergeAlgorithm)
        : base(name, algorithm, repository)
    {
        _mergeAlgorithm = mergeAlgorithm ?? throw new BackupsExtraException("Merge algorithm is null.");
        _filterAlgorithms = new List<IFilterAlgorithm>();
        Logger.Log("BackupExtra Task was created.");
    }

    public static BackupExtraTask Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new BackupsExtraException("JSON is empty.");
        BackupExtraTask backupExtraTask = JsonSerializer.Deserialize<BackupExtraTask>(json) ??
                                          throw new BackupsExtraException("Backup Task is null.");
        Logger.Log("BackupExtra Task was created from JSON.");
        return backupExtraTask;
    }

    public string Serialize()
    {
        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(Repository.Location + "Config.txt", json);
        Logger.Log("BackupExtra Task settings were converted to JSON.");
        return json;
    }

    public void AddFilter(IFilterAlgorithm filter)
    {
        if (filter is null)
            throw new BackupsExtraException("Filter algorithm is null.");
        _filterAlgorithms.Add(filter);
        Logger.Log("Filter added.");
    }

    public IReadOnlyList<RestorePoint> Filter()
    {
        IReadOnlyList<RestorePoint> restorePoints = RestorePoints;
        foreach (IFilterAlgorithm filter in _filterAlgorithms)
            restorePoints = filter.Filter(restorePoints);
        Logger.Log("Filtered.");
        return restorePoints;
    }

    public void RestoreDataToOriginalLocation(int numberOfRestorePoint)
    {
        if (numberOfRestorePoint < MinNumberOfRestorePoint)
            throw new BackupsExtraException($"Number of restore point can't be less than {MinNumberOfRestorePoint}");
        if (numberOfRestorePoint > RestorePoints.Count)
            throw new BackupsExtraException($"There are only {RestorePoints.Count} points.");
        string repository = Repository.Location + $"/{Name}" + $"/RestorePoint_{numberOfRestorePoint}";
        RestorePoint restorePoint = RestorePoints[numberOfRestorePoint];
        foreach (Storage storage in restorePoint.Storages)
        {
            foreach (BackupObject backupObject in storage.BackupObjects)
            {
                string? originalLocation = Path.GetDirectoryName(backupObject.Path);
                if (string.IsNullOrWhiteSpace(originalLocation))
                    throw new BackupsExtraException("Original Location is empty.");
                if (!Directory.Exists(originalLocation))
                    Directory.CreateDirectory(originalLocation);
                string zipLocation = Path.Combine(repository, storage.PathToFile);
                ZipFile zipFile = ZipFile.Read(zipLocation);
                zipFile.ExtractAll(originalLocation);
            }
        }

        Logger.Log("Date restored to original location.");
    }

    public void RestoreDataToCustomLocation(int numberOfRestorePoint, string path)
    {
        if (numberOfRestorePoint < MinNumberOfRestorePoint)
            throw new BackupsExtraException($"Number of restore point can't be less than {MinNumberOfRestorePoint}");
        if (numberOfRestorePoint < RestorePoints.Count)
            throw new BackupsExtraException($"There are only {RestorePoints.Count} points.");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsExtraException("Path is empty.");
        string repository = Repository.Location + $"/{Name}" + $"/RestorePoint_{numberOfRestorePoint}";
        RestorePoint restorePoint = RestorePoints[numberOfRestorePoint];
        foreach (Storage storage in restorePoint.Storages)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new BackupsExtraException("Original Location is empty.");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string zipLocation = Path.Combine(repository, storage.PathToFile);
            ZipFile zipFile = ZipFile.Read(zipLocation);
            zipFile.ExtractAll(path);
        }

        Logger.Log("Data restored to custom location.");
    }

    public void Merge(int numberOfRestorePoint1, int numberOfRestorePoint2)
    {
        if (numberOfRestorePoint1 < MinNumberOfRestorePoint || numberOfRestorePoint2 < MinNumberOfRestorePoint)
            throw new BackupsExtraException($"Number of restore point can't be less than {MinNumberOfRestorePoint}");
        if (numberOfRestorePoint1 > RestorePoints.Count || numberOfRestorePoint2 > RestorePoints.Count)
            throw new BackupsExtraException($"There are only {RestorePoints.Count} points.");
        RestorePoint restorePoint1 = RestorePoints[numberOfRestorePoint1];
        RestorePoint restorePoint2 = RestorePoints[numberOfRestorePoint2];
        RestorePoint newRestorePoint = _mergeAlgorithm.Merge(restorePoint1, restorePoint2);
        RemoveRestorePoint(restorePoint1);
        RemoveRestorePoint(restorePoint2);
        AddRestorePoint(newRestorePoint);
        Logger.Log("Points were merged.");
    }
}