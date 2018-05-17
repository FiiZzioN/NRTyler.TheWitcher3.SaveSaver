// ***********************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
//
// Author           : Nicholas Tyler
// Created          : 05-16-2018
//
// Last Modified By : Nicholas Tyler
// Last Modified On : 05-17-2018
//
// License          : MIT License
// ***********************************************************************

using System;

namespace NRTyler.TheWitcher3.SaveSaver
{
    public class Program
    {
        private static void Main()
        {
            var settings = GrabSettingsFile();
            var backupCreator = new BackupCreator(settings);

            backupCreator.CreateBackup();
        }

        private static ApplicationSettings GrabSettingsFile()
        {
            var currentDirectory = Environment.CurrentDirectory;
            //var settignsPath     = $"{currentDirectory}/{ApplicationSettings.FileName}";
            var settingsRepo     = new ApplicationSettingsRepo(currentDirectory);

            return settingsRepo.Retrieve();
        }

        private static void Write(object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
