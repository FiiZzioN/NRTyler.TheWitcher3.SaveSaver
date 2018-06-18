// ***********************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
//
// Author           : Nicholas Tyler
// Created          : 05-16-2018
//
// Last Modified By : Nicholas Tyler
// Last Modified On : 06-18-2018
//
// License          : MIT License
// ***********************************************************************

using System;
using System.IO.Compression;
using System.Reflection;
using NRTyler.CodeLibrary.Utilities;

namespace NRTyler.TheWitcher3.SaveSaver
{
    public class Program
    {
        private static ApplicationSettings Settings = GrabSettingsFile();

        private static void Main()
        {
            Console.Title = Assembly.GetExecutingAssembly().GetName().Name;

            Write("Creating a backup!");
            Write();

            CreateBackup();

            var numberOfBackupsRemoved = PruneOldBackups();

            WritePruneMessage(numberOfBackupsRemoved);
            InitiateClosingSequence();
        }

        private static ApplicationSettings GrabSettingsFile()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var settingsRepo = new ApplicationSettingsRepo(currentDirectory);

            return settingsRepo.Retrieve();
        }

        private static void CreateBackup()
        {
            var backupCreator = new BackupCreator(Settings);

            backupCreator.CreateBackup(CompressionLevel.Optimal);

            Write("Backup created!");
            Write();
        }

        private static int PruneOldBackups()
        {
            var backupCleaner = new BackupCleaner(Settings);
            var allBackups    = backupCleaner.GetAllEntries();

            return backupCleaner.PruneOldestEntries(allBackups);
        }

        private static void InitiateClosingSequence()
        {
            // This is equivalent to 20 seconds
            var timeSpan = new TimeSpan(0, 0, 5);
            
            AppTerminator.CloseAfterElapsedTime(timeSpan, true);
            AppTerminator.CloseApplication(true);
        }

        private static void WritePruneMessage(int numberOfBackupsRemoved)
        {
            var removedMessage = numberOfBackupsRemoved > 1
                ? $"{numberOfBackupsRemoved} backups have been removed."
                : $"{numberOfBackupsRemoved} backup has been removed.";
            
            var standardMessage = "No backups need to be removed.";

            Write(numberOfBackupsRemoved > 0 ? removedMessage : standardMessage);
        }

        private static void Write(object obj = null)
        {
            Console.WriteLine(obj);
        }
    }
}
