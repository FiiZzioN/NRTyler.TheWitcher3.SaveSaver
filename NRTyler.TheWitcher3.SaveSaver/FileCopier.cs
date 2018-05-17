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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NRTyler.TheWitcher3.SaveSaver
{
    public class FileCopier
    {
        public FileCopier(ApplicationSettings applicationSettings)
        {
            ApplicationSettings = applicationSettings;
        }

        private ApplicationSettings ApplicationSettings { get; }


        /// <summary>
        /// Scans the save game location and returns an enumerable collection containing
        /// <see cref="FileInfo"/> objects for the files that need to be backed up.
        /// </summary>
        public IEnumerable<FileInfo> ScanSaveGameLocation()
        {
            // PNG is the saved games preview picture, and SAV files are the actual save games
            var acceptableExtensions = new [] {".png", ".sav"};

            var directoryInfo = new DirectoryInfo(ApplicationSettings.SaveLocation);
            var filesToBackup = new List<FileInfo>();

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (CheckFileExtension(file.Extension, acceptableExtensions))
                {
                    filesToBackup.Add(file);
                }
            }

            return filesToBackup;
        }

        /// <summary>
        /// Copies the specified files to a specified directory.
        /// </summary>
        /// <param name="filesToCopy">The full paths of the files that are going to be copied.</param>
        /// <param name="directoryInfo">
        /// The <see cref="DirectoryInfo"/> object to the
        ///  directory where the files are going to be located.
        /// </param>
        public void CopyFiles(IEnumerable<FileInfo> filesToCopy, DirectoryInfo directoryInfo)
        {
            foreach (var file in filesToCopy)
            {
                var fileName = $"{directoryInfo.FullName}/{file.Name}";
                file.CopyTo(fileName);
            }
        }

        public static bool CheckFileExtension(string filePath, string desiredExtension)
        {
            return CheckFileExtension(filePath, new[] {$"{desiredExtension}"});
        }

        public static bool CheckFileExtension(string filePath, IEnumerable<string> desiredExtensions)
        {
            var fileExtension = Path.GetExtension(filePath);

            return desiredExtensions.Contains(fileExtension);
        }
    }
}