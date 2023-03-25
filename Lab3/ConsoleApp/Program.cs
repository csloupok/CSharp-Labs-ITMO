using Backups.Algorithms;
using Backups.Models;

namespace ConsoleApp1;

internal static class Program
{
    private static void Main()
    {
        const string tempPath = @"/Users/eldarkasymov/tempFiles";
        const string repoPath = @"/Users/eldarkasymov/repository";
        Repository repository = new Repository(repoPath);
        IStorageAlgorithm algorithm = new SingleStorageAlgorithm(tempPath);
        BackupTask task = new BackupTask("kek", algorithm, repository);
        BackupObject object1 = new BackupObject(@"/Users/eldarkasymov/backuppp/1.rtf");
        BackupObject object2 = new BackupObject(@"/Users/eldarkasymov/backuppp/2.rtf");

        task.AddBackupObject(object1);
        task.AddBackupObject(object2);
        task.CreateBackup();

        Console.WriteLine(task.RestorePoints.First().Storages[0].PathToFile);
    }
}