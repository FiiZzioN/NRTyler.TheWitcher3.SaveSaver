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
using System.Runtime.Serialization;
using NRTyler.CodeLibrary.Extensions;

namespace NRTyler.TheWitcher3.SaveSaver
{
    [Serializable]
    [DataContract(Name = "ApplicationSettings")]
    public class ApplicationSettings
    {
        public ApplicationSettings()
        {
            this.saveLocation   = String.Empty;
            this.backupLocation = String.Empty;
            this.backupsToKeep  = 15;
        }

        private string saveLocation;
        private string backupLocation;
        private int backupsToKeep;

        /// <summary>
        /// The file name that this settings file, once serialized, will be saved as.
        /// The file type designation is left up to the user.
        /// </summary>
        public const string FileName = "Settings";

        /// <summary>
        /// Gets or sets the directory where the game saves are located.
        /// </summary>
        [DataMember]
        public string SaveLocation
        {
            get { return this.saveLocation; }
            set
            {
                if (value.IsNullOrWhiteSpace()) return;
                this.saveLocation = value;                
            }
        }

        /// <summary>
        /// Gets or sets the directory where the game save backups are going to be located.
        /// </summary>
        [DataMember]
        public string BackupLocation
        {
            get { return this.backupLocation; }
            set
            {
                if (value.IsNullOrWhiteSpace()) return;
                this.backupLocation = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of backups to keep before the oldest start to get pruned away.
        /// </summary>
        [DataMember]
        public int BackupsToKeep
        {
            get { return this.backupsToKeep; }
            set
            {
                if (value <= 0) return;
                this.backupsToKeep = value;
            }
        }
    }
}