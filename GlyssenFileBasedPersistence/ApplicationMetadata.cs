﻿using System;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using SIL.Xml;

namespace GlyssenFileBasedPersistence
{
	public interface IDataVersionInfo
	{
		int DataVersion { get; set; }
	}

	[XmlRoot("ProtoscriptGeneratorMetadata")]
	public class ApplicationMetadata : IDataVersionInfo
	{
		private const string kFilename = "ApplicationMetadata.xml";
		internal const int kDataFormatVersion = 4;

		[XmlAttribute("dataVersion")]
		public int DataVersion { get; set; }

		[XmlElement("InactiveParatextProject", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
		public string[] InactiveUnstartedParatextProjects { get; set; }

		public static ApplicationMetadata Load(out Exception exception)
		{
			exception = null;

			ApplicationMetadata metadata;
			if (!File.Exists(FilePath))
			{
				metadata = new ApplicationMetadata();
				if (!Directory.Exists(ProjectRepository.ProjectsBaseFolder))
				{
					// In production, Installer is responsible for creating the base data folder.
					// The version number will be initially set to 0, but since there won't be any
					// projects to migrate, the migrator won't do anything but set the version number.
					// However, on a developer machine (or in the event that a user has blown away or
					// renamed the folder), we need to force its creation now.
					Directory.CreateDirectory(ProjectRepository.ProjectsBaseFolder);
					metadata.DataVersion = kDataFormatVersion;
					metadata.Save();
				}
			}
			else
				metadata = XmlSerializationHelper.DeserializeFromFile<ApplicationMetadata>(FilePath, out exception);
			return metadata;
		}

		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(FilePath, this);
		}

		private static string FilePath => Path.Combine(ProjectRepository.ProjectsBaseFolder, kFilename);
	}
}
