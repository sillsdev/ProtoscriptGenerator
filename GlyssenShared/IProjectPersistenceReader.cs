﻿using System.IO;

namespace Glyssen.Shared
{
	public interface IProjectPersistenceReader
	{
		bool ProjectExists(string languageIsoCode, string metadataId, string name);
		bool ResourceExists(IProject project, ProjectResource resource);
		bool BookResourceExists(IProject project, string bookId);
		/// <summary>
		/// Client is responsible for disposing the TextReader when done with it.
		/// </summary>
		TextReader Load(IProject project, ProjectResource resource);
		TextReader LoadBook(IProject project, string bookId);
	}
}
