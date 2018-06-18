// ***********************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
//
// Author           : Nicholas Tyler
// Created          : 05-17-2018
//
// Last Modified By : Nicholas Tyler
// Last Modified On : 06-18-2018
//
// License          : MIT License
// ***********************************************************************

using System.IO.Compression;

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
        /// Creates a backup of the game files.
        /// </summary>
        /// <param name="compressionLevel"> Specifies whether the compression operation emphasizes speed or compression size.</param>
        public void CreateBackup(CompressionLevel compressionLevel)
        {
            var validDate = BackupLabeller.GetValidDateFormat();
            var validTime = BackupLabeller.GetValidTimeFormat(true);

            var backupName = $"Backup for {validDate} at {validTime}";

            var source      = ApplicationSettings.SaveLocation;
            var destination = $"{ApplicationSettings.BackupLocation}/{backupName}.zip";

            ZipFile.CreateFromDirectory(source, destination, compressionLevel, false);
        }
    }
}