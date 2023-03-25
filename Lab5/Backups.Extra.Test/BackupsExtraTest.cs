using Backups.Algorithms;
using Backups.Extra.Algorithms;
using Backups.Extra.Models;
using Backups.Models;
using Xunit;

namespace Backups.Extra.Test;

public class BackupsExtraTest
{
    [Fact]
    public void FilterByAmount_CheckAmount()
    {
        const int amountSetting = 3;
        const string tempPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles";
        const string repoPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles/Repo";
        Repository repository = new Repository(repoPath);
        IStorageAlgorithm algorithm = new SplitStorageAlgorithm(tempPath);
        SplitMergeAlgorithm mergeAlgorithm = new SplitMergeAlgorithm();
        IFilterAlgorithm filterAlgorithm = new FilterByAmountAlgorithm(amountSetting);
        BackupExtraTask task = new BackupExtraTask("Task", algorithm, repository, mergeAlgorithm);
        BackupObject object1 = new BackupObject(@"/bin/cat");
        BackupObject object2 = new BackupObject(@"/bin/sleep");

        task.AddFilter(filterAlgorithm);
        task.AddBackupObject(object1);
        task.CreateBackup();
        task.AddBackupObject(object2);
        task.CreateBackup();
        task.CreateBackup();
        task.CreateBackup();
        task.CreateBackup();

        Assert.Equal(amountSetting, task.Filter().Count);
    }

    [Fact]
    public void FilterByDate_CheckAmount()
    {
        const string tempPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles";
        const string repoPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles/Repo";
        Repository repository = new Repository(repoPath);
        IStorageAlgorithm algorithm = new SplitStorageAlgorithm(tempPath);
        SplitMergeAlgorithm mergeAlgorithm = new SplitMergeAlgorithm();
        IFilterAlgorithm filterAlgorithm = new FilterByDateAlgorithm(DateTime.Today);
        BackupExtraTask task = new BackupExtraTask("Task", algorithm, repository, mergeAlgorithm);
        BackupObject object1 = new BackupObject(@"/bin/bash");
        BackupObject object2 = new BackupObject(@"/bin/cp");

        task.AddFilter(filterAlgorithm);
        task.AddBackupObject(object1);
        task.CreateBackup();
        task.AddBackupObject(object2);
        task.CreateBackup(DateTime.MinValue);
        task.CreateBackup(DateTime.MinValue);
        task.CreateBackup(DateTime.MinValue);
        task.CreateBackup(DateTime.MinValue);

        Assert.Equal(1, task.Filter().Count);
    }

    [Fact]
    public void MergePoints_CheckAmountAndList()
    {
        const string tempPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles";
        const string repoPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles/Repo";
        Repository repository = new Repository(repoPath);
        IStorageAlgorithm algorithm = new SplitStorageAlgorithm(tempPath);
        SplitMergeAlgorithm mergeAlgorithm = new SplitMergeAlgorithm();
        IFilterAlgorithm filterAlgorithm = new FilterByDateAlgorithm(DateTime.Today);
        BackupExtraTask task = new BackupExtraTask("Task", algorithm, repository, mergeAlgorithm);
        BackupObject object1 = new BackupObject(@"/bin/ls");
        BackupObject object2 = new BackupObject(@"/bin/ps");

        task.AddFilter(filterAlgorithm);
        task.AddBackupObject(object1);
        task.CreateBackup();
        task.AddBackupObject(object2);
        task.CreateBackup();
        task.Merge(0, 1);

        Assert.Equal(1, task.RestorePoints.Count);
        Assert.Equal(2, task.RestorePoints[0].Storages.Count);
    }
}