// ***********************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
//
// Author           : Nicholas Tyler
// Created          : 05-16-2018
//
// Last Modified By : Nicholas Tyler
// Last Modified On : 03-29-2018
//
// License          : MIT License
// ***********************************************************************

using NRTyler.CodeLibrary.Interfaces;
using NRTyler.CodeLibrary.Interfaces.Generic;
using NRTyler.CodeLibrary.Utilities;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace NRTyler.TheWitcher3.SaveSaver
{
    /// <summary>
    /// Handles the creation, retrieval, updating, and deletion of <see cref="ApplicationSettings"/> objects, also known as this application's settings file.
    /// </summary>
    /// <seealso cref="ApplicationSettings" />
    /// <seealso cref="ApplicationSettings" />
    public sealed class ApplicationSettingsRepo : IDataContractRepository<ApplicationSettings>, ICrudRepository<ApplicationSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsRepo"/> class.
        /// </summary>
        /// <param name="path">The directory where the settings file is located.</param>
        public ApplicationSettingsRepo(string path) : this(path, new ErrorReporter(true))
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsRepo" /> class.
        /// </summary>
        /// <param name="path">The directory where the settings file is located.</param>
        /// <param name="errorDialogService">The dialog service that'll be used when an error occurs.</param>
        public ApplicationSettingsRepo(string path, IErrorDialogService errorDialogService)
        {
            Path               = path;
            ErrorDialogService = errorDialogService;
            DCSerializer       = new DataContractSerializer(typeof(ApplicationSettings));
        }

        #region Fields, Properties

        private DataContractSerializer dcSerializer;

        /// <summary>
        /// Gets or sets the <see cref="T:System.Runtime.Serialization.DataContractSerializer" />.
        /// </summary>
        public DataContractSerializer DCSerializer
        {
            get { return this.dcSerializer; }
            set
            {
                if (value == null) return;
                this.dcSerializer = value;
            }
        }

        /// <summary>
        /// Gets or sets the path where the settings file will be located.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets or sets the service that shows the error reporting dialog boxes.
        /// </summary>
        private IErrorDialogService ErrorDialogService { get; }

        #endregion

        /// <summary>
        /// Serializes an <see cref="T:System.Object" /> using the specified <see cref="T:System.IO.Stream" />.
        /// </summary>
        /// <param name="stream">The <see cref="T:System.IO.Stream" /> that the <see cref="T:System.Object" /> will be serialized to.</param>
        /// <param name="obj">The <see cref="T:System.Object" /> being serialized.</param>
        public void Serialize(Stream stream, ApplicationSettings obj)
        {
            var xmlWriter = XmlWriter.Create(stream, RetrieveXMLWriterSettings());

            using (xmlWriter)
            {
                DCSerializer.WriteObject(xmlWriter, obj);
            }
        }

        /// <summary>
        /// Deserializes an <see cref="T:System.Object" /> using the specified <see cref="T:System.IO.Stream" />.
        /// </summary>
        /// <param name="stream">The <see cref="T:System.IO.Stream" /> that the <see cref="T:System.Object" /> is being deserialized from.</param>
        /// <returns>The deserialized <see cref="T:System.Object" />.</returns>
        public ApplicationSettings Deserialize(Stream stream)
        {
            using (stream)
            {
                return (ApplicationSettings)DCSerializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Creates the application setting's XML file.
        /// </summary>
        /// <param name="obj">The application settings you want to serialize.</param>
        public void Create(ApplicationSettings obj)
        {
            var message  = String.Empty;
            var fileName = ApplicationSettings.FileName;
            var path     = $"{Path}/{fileName}.xml";

            // This methods job is to create new objects, not replace them. 
            // If the file already exists, then that's what the update method is for.
            // If you need to replace the file, delete it and then create it.
            if (File.Exists(path))
            {
                message =
                    "A file with that name already exists. If you wish to update the file, call the Update() method. " +
                    "If you want to replace the file, call the Delete() method and then try to create the file again.";
                ErrorDialogService.Show(message);
                return;
            }

            var fileStream = CreateSettingsFileStream(obj);

            using (fileStream)
            {
                Serialize(fileStream, obj);
            }
        }

        /// <summary>
        /// Retrieves an application settings with the specified name / key.
        /// </summary>
        /// <param name="key">The application settings file name.</param>
        public ApplicationSettings Retrieve(string key = ApplicationSettings.FileName)
        {
            var message = String.Empty;
            var path    = $"{Path}/{key}.xml";

            FileStream stream = null;

            try
            {
                stream = File.OpenRead(path);
            }
            catch (DirectoryNotFoundException)
            {
                message = "The setting's directory couldn't be found because the path was invalid (for example, it's on an unmapped drive).";
                ErrorDialogService.Show(message);
            }
            catch (FileNotFoundException)
            {
                message = "The setting's XML file couldn't be found.";
                ErrorDialogService.Show(message);
            }
            catch (PathTooLongException)
            {
                message = "The setting's XML file couldn't be retrieved as the resulting path would be too long.";
                ErrorDialogService.Show(message);
            }
            catch (IOException)
            {
                message = "The setting's XML file couldn't be retrieved because an Input / Output error occurred while opening the file.";
                ErrorDialogService.Show(message);
            }
            catch (UnauthorizedAccessException)
            {
                message = "The setting's XML file couldn't be retrieved because this application doesn't have access to the destination.";
                ErrorDialogService.Show(message);
            }
            catch (NotSupportedException)
            {
                message = "The setting's XML file couldn't be retrieved because the path was in an invalid format.";
                ErrorDialogService.Show(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorDialogService.Show(e.Message);
                throw;
            }

            using (stream)
            {
                return Deserialize(stream);
            }
        }

        /// <summary>
        /// Updates an already existing application settings file with the specified application settings.
        /// </summary>
        /// <param name="obj">The new settings you wish to replace the old settings with.</param>
        public void Update(ApplicationSettings obj)
        {
            var message  = String.Empty;
            var fileName = ApplicationSettings.FileName;
            var path     = $"{Path}/{fileName}.xml";

            if (!File.Exists(path))
            {
                message = "The settings file doesn't exist so there's nothing to update.";
                ErrorDialogService.Show(message);
                return;
            }

            var fileStream = CreateSettingsFileStream(obj);

            using (fileStream)
            {
                Serialize(fileStream, obj);
            }
        }

        /// <summary>
        /// Deletes the application settings file with the specified name / key.
        /// </summary>
        /// <param name="key">The application settings file name.</param>
        public void Delete(string key = ApplicationSettings.FileName)
        {
            var message = String.Empty;
            var path = $"{Path}/{key}.xml";

            try
            {
                File.Delete(path);
            }
            catch (DirectoryNotFoundException)
            {
                message = "The settings's directory couldn't be deleted because the path was invalid (for example, it's on an unmapped drive or it couldn't be found).";
                ErrorDialogService.Show(message);
            }
            catch (PathTooLongException)
            {
                message = "The setting's file couldn't be deleted because the resulting path would be too long.";
                ErrorDialogService.Show(message);
            }
            catch (IOException)
            {
                message = "The setting's file couldn't be deleted because it's the applications current " +
                          "working directory, being used by another process, or contains a read-only file.";
                ErrorDialogService.Show(message);
            }
            catch (UnauthorizedAccessException)
            {
                message = "The setting's file couldn't be deleted because this application doesn't have the proper permissions.";
                ErrorDialogService.Show(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorDialogService.Show(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the XML file where the application settings's information is held. 
        /// Returns the <see cref="FileStream"/> of the file being created.
        /// </summary>
        /// <param name="obj">The <see cref="ApplicationSettings"/> object that this method uses to gather its information.</param>
        private FileStream CreateSettingsFileStream(ApplicationSettings obj)
        {
            var message  = String.Empty;
            var fileName = ApplicationSettings.FileName;
            var path     = $"{Path}/{fileName}.xml";

            FileStream stream = null;

            try
            {
                stream = File.Create(path);
            }
            catch (DirectoryNotFoundException)
            {
                message = "The settings file couldn't be created because the path was invalid (for example, it's on an unmapped drive).";
                ErrorDialogService.Show(message);
            }
            catch (PathTooLongException)
            {
                message = "The settings file couldn't be created because the resulting path would be too long.";
                ErrorDialogService.Show(message);
            }
            catch (IOException)
            {
                message = $"The settings file couldn't be created because the specified path was a file, or the network name isn't known.";
                ErrorDialogService.Show(message);
            }
            catch (UnauthorizedAccessException)
            {
                message = "The settings file couldn't be created because this application doesn't have access to the destination.";
                ErrorDialogService.Show(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorDialogService.Show(e.Message);
                throw;
            }

            return stream;
        }

        /// <summary>
        /// Retrieves the standard XML writer settings that this repo uses.
        /// </summary>
        private XmlWriterSettings RetrieveXMLWriterSettings()
        {
            return new XmlWriterSettings
            {
                CloseOutput             = true,
                Indent                  = true,
                NewLineOnAttributes     = true,
                WriteEndDocumentOnClose = true
            };
        }
    }
}