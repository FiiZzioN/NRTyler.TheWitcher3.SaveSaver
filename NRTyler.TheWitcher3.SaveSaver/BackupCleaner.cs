// ************************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
// 
// Author           : Nicholas Tyler
// Created          : 05-16-2018
// 
// Last Modified By : Nicholas Tyler
// Last Modified On : 05-16-2018
// 
// License          : MIT License
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NRTyler.TheWitcher3.SaveSaver
{
    public class BackupCleaner
    {
        public BackupCleaner(ApplicationSettings applicationSettings)
        {
            ApplicationSettings = applicationSettings;
        }

        private ApplicationSettings ApplicationSettings { get; }


        /// <summary>
        /// Returns an enumerable collection of <see cref="FileInfo"/> 
        /// objects based on all of the backups located in the backup location.
        /// </summary>
        /// <returns>IEnumerable&lt;FileInfo&gt;.</returns>
        public IEnumerable<FileInfo> GetAllEntries()
        {
            var backupLocation = ApplicationSettings.BackupLocation;
            var directoryInfo  = new DirectoryInfo(backupLocation);

            return directoryInfo.EnumerateFiles();
        }

        /// <summary>
        /// Prunes the oldest backup entries. Only does so when the number of backups exceed 
        /// the specified number of backups to keep. Returns the total number of backups removed.
        /// </summary>
        /// <param name="collection">The collection of <see cref="FileInfo"/> objects that this method will operate on. </param>
        public int PruneOldestEntries(IEnumerable<FileInfo> collection)
        {
            var numberOfFilesRemoved = 0;

            while (true)
            {
                var allFiles = collection.ToList();

                // No need to remove backups if there are less than or equal to the specified limit.
                if (allFiles.Count <= ApplicationSettings.BackupsToKeep)
                {
                    break;
                }

                var oldestFile = default(FileInfo);
                var oldestFileLastWriteTime = DateTime.UtcNow;

                foreach (var file in allFiles)
                {
                    var currentFile = file;
                    var currentFileLastWriteTime = file.LastWriteTimeUtc;

                    // Greater than zero means t1 is later than t2
                    if (DateTime.Compare(currentFileLastWriteTime, oldestFileLastWriteTime) < 0)
                    {
                        oldestFile = currentFile;
                        oldestFileLastWriteTime = currentFileLastWriteTime;
                    }
                }

                // Remove the oldest file from the pool.
                allFiles.Remove(oldestFile);

                // Delete the file.
                oldestFile?.Delete();
                numberOfFilesRemoved++;

                // Start again until we hit our limit.
                collection = allFiles;
            }

            return numberOfFilesRemoved;
        }
    }
}