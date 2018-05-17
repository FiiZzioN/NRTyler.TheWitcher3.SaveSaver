// ************************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
// 
// Author           : Nicholas Tyler
// Created          : 05-17-2018
// 
// Last Modified By : Nicholas Tyler
// Last Modified On : 05-17-2018
// 
// License          : MIT License
// ***********************************************************************

using NRTyler.CodeLibrary.Extensions;
using System.IO;

namespace NRTyler.TheWitcher3.SaveSaver
{
    public class BackupCreator
    {
        public BackupCreator(ApplicationSettings applicationSettings)
        {
            ApplicationSettings = applicationSettings;
        }

        private ApplicationSettings ApplicationSettings { get; }

        /// <summary>
        /// Ensures that a folder labeled with the current day's date 
        /// will be present so the backups can be stored properly.
        /// </summary>
        public DirectoryInfo CreateTodaysFolder()
        {
            var path     = ApplicationSettings.BackupLocation;
            var fullPath = $"{path}/{BackupLabeller.GetValidDateFormat()}";

            return DirectoryEx.CreateDirectoryIfNonexistent(fullPath);
        }

        /// <summary>
        /// Creates a sub-folder that's labeled with the current time. 
        /// This allows the user to know when the backups were made.
        /// </summary>
        /// <param name="directoryInfo">The directory where this sub-folder will be created.</param>
        /// <returns>DirectoryInfo.</returns>
        public DirectoryInfo CreateTimeSubfolder(DirectoryInfo directoryInfo)
        {
            var fullPath = $"{directoryInfo.FullName}/{BackupLabeller.GetValidTimeFormat()}";

            return DirectoryEx.CreateDirectoryIfNonexistent(fullPath);
        }

        /// <summary>
        /// Creates a backup of the game files.
        /// </summary>
        public void CreateBackup()
        {
            // Create the folder and sub-folder where the backups will be located.
            var todaysDirectory  = CreateTodaysFolder();
            var timeSubdirectory = CreateTimeSubfolder(todaysDirectory);

            // Instantiate the file copier and scan for what items need to be backed up.
            var fileCopier  = new FileCopier(ApplicationSettings);
            var filesToCopy = fileCopier.ScanSaveGameLocation();

            // Cope the files.
            fileCopier.CopyFiles(filesToCopy, timeSubdirectory);
        }
    }
}