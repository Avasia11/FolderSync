namespace FolderSync
{
    using System;
    using System.IO;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: FolderSync <sourceFolder> <replicaFolder> <intervalInSeconds> <logFilePath>");
                return;
            }

            string sourceFolder = args[0];
            string replicaFolder = args[1];
            if (!int.TryParse(args[2], out int intervalInSeconds))
            {
                Console.WriteLine("Invalid interval specified. Please provide an integer value for the interval in seconds.");
                return;
            }
            string logFilePath = args[3];

            // Verify if the folders exist
            if (!Directory.Exists(sourceFolder) || !Directory.Exists(replicaFolder))
            {
                Console.WriteLine("One or both of the specified folders do not exist.");
                return;
            }

            // Initialize log
            using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
            {
                logWriter.WriteLine($"Synchronization started at {DateTime.Now}");
            }

            // Synchronization loop
            while (true)
            {
                SynchronizeFolders(sourceFolder, replicaFolder, logFilePath);
                Thread.Sleep(intervalInSeconds * 1000);
            }
        }

        static void SynchronizeFolders(string sourceFolder, string replicaFolder, string logFilePath)
        {
            var sourceDir = new DirectoryInfo(sourceFolder);
            var replicaDir = new DirectoryInfo(replicaFolder);

            // Synchronize files from source folder
            foreach (var file in sourceDir.GetFiles())
            {
                string targetFilePath = Path.Combine(replicaFolder, file.Name);
                if (!File.Exists(targetFilePath) || File.GetLastWriteTimeUtc(targetFilePath) != file.LastWriteTimeUtc)
                {
                    file.CopyTo(targetFilePath, true);
                    LogAction(logFilePath, $"File copied: {file.FullName} -> {targetFilePath}");
                }
            }

            // Delete files from replica folder that no longer exist in source
            foreach (var file in replicaDir.GetFiles())
            {
                string sourceFilePath = Path.Combine(sourceFolder, file.Name);
                if (!File.Exists(sourceFilePath))
                {
                    file.Delete();
                    LogAction(logFilePath, $"File deleted: {file.FullName}");
                }
            }

            // Synchronize subfolders recursively
            foreach (var dir in sourceDir.GetDirectories())
            {
                string targetDirPath = Path.Combine(replicaFolder, dir.Name);
                if (!Directory.Exists(targetDirPath))
                {
                    Directory.CreateDirectory(targetDirPath);
                    LogAction(logFilePath, $"Directory created: {targetDirPath}");
                }
                SynchronizeFolders(dir.FullName, targetDirPath, logFilePath);
            }

            // Delete directories from replica folder that no longer exist in source
            foreach (var dir in replicaDir.GetDirectories())
            {
                string sourceDirPath = Path.Combine(sourceFolder, dir.Name);
                if (!Directory.Exists(sourceDirPath))
                {
                    dir.Delete(true);
                    LogAction(logFilePath, $"Directory deleted: {dir.FullName}");
                }
            }
        }

        static void LogAction(string logFilePath, string message)
        {
            using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
            {
                logWriter.WriteLine($"{DateTime.Now}: {message}");
            }
            Console.WriteLine(message);
        }
    }
}
