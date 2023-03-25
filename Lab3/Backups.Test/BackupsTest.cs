using Backups.Algorithms;
using Backups.Models;
using Xunit;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void TestCase1()
    {
        const string tempPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/TempFiles";
        const string repoPath = @"/home/runner/work/csloupok/csloupok/Lab3/Backups/TestDirectory/Repository";
        Repository repository = new Repository(repoPath);
        IStorageAlgorithm algorithm = new SplitStorageAlgorithm(tempPath);
        BackupTask task = new BackupTask("kek", algorithm, repository);
        BackupObject object1 = new BackupObject(@"/bin/cat");
        BackupObject object2 = new BackupObject(@"/bin/sleep"); // котики спят

        task.AddBackupObject(object1);
        task.AddBackupObject(object2);
        task.CreateBackup();
        task.RemoveBackupObject(object2);
        task.CreateBackup();

        Assert.Equal(2, task.RestorePoints.Count);
        Assert.Equal(3, task.RestorePoints[0].Storages.Count + task.RestorePoints[1].Storages.Count);
    }
}